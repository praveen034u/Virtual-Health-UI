function speak(text, delayAfterSpeech = 50) {
    return new Promise(resolve => {
        const msg = new SpeechSynthesisUtterance(text);
        msg.lang = "en-US";
        msg.onend = () => setTimeout(resolve, delayAfterSpeech);
        speechSynthesis.cancel(); // Clear queue
        speechSynthesis.speak(msg);
    });
}

function recognizeSpeech() {
    return new Promise(resolve => {
        const recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
        recognition.lang = 'en-US';
        recognition.interimResults = false;
        recognition.maxAlternatives = 1;

        recognition.onresult = (event) => {
            const result = event.results[0][0].transcript.trim();
            console.log("🎤 Raw Speech Input:", result);
            resolve(result);
        };

        recognition.onerror = (event) => {
            console.warn("❌ Speech error:", event.error);
            resolve('');
        };

        recognition.start();
    });
}

window.openAccordionSection = function (sectionId) {
    const all = document.querySelectorAll('.accordion-collapse');
    all.forEach(sec => sec.classList.remove('show'));

    const target = document.getElementById(sectionId);
    if (target) {
        target.classList.add('show');
    }
};

window.startVoiceAssistantIntro = async function (dotNetHelper) {
    await speak("Let's fill your personal details.");
    window.openAccordionSection("collapsePersonal");

    const fields = [
        { question: "What is your first name?", field: "FirstName" },
        { question: "What is your last name?", field: "LastName" },
        { question: "What is your date of birth? For example, say January first 2000 or zero one zero one 2000.", field: "BirthDate", isDate: true },
        { question: "What is your gender? Say male, female, other, or unknown.", field: "Gender" },
        { question: "What is your phone number?", field: "PhoneNumber" }
    ];

    for (const f of fields) {
        let attempt = 0;
        let answer = '';

        while (attempt < 2 && !answer) {
            const delay = f.field === "FirstName" ? 100 : 50;
            const prompt = attempt === 0 ? f.question : "Sorry, I didn't catch that. " + f.question;

            await speak(prompt, delay);
            answer = await recognizeSpeech();
            attempt++;
        }

        if (answer) {
            let clean = f.isDate ? parseDate(answer) : answer;

            if (f.field === "Gender") {
                const lower = clean.toLowerCase();
                if (lower.includes("mail") || lower.includes("male")) clean = "male";
                else if (lower.includes("femail") || lower.includes("female")) clean = "female";
                else if (lower.includes("other")) clean = "other";
                else if (lower.includes("unknown")) clean = "unknown";
            }

            if (f.field === "BirthDate" && (!clean || clean === answer)) {
                console.warn("⚠️ Invalid DOB, setting to default.");
                clean = "2000-01-01";
                await speak("You gave an invalid birth date. Setting it to January 1st 2000.");
            }

            console.log("📝 Field:", f.field, "→", clean);
            await dotNetHelper.invokeMethodAsync("SetSpeechInput", f.field, clean);
        }
    }

    await speak("Done filling personal details. Let's fill the Address now.");
    await window.startVoiceAssistantAddress(dotNetHelper);
};

window.startVoiceAssistantAddress = async function (dotNetHelper) {
    await speak("Now let's fill your address details.");
    window.openAccordionSection("collapseAddress");

    const fields = [
        { question: "What is your address line one?", field: "AddressLine1" },
        { question: "What is your street name?", field: "Street" },
        { question: "Which city do you live in?", field: "City" },
        { question: "What is your state?", field: "State" },
        { question: "What is your zip code?", field: "ZipCode" },
        { question: "What is your country? If it's not the United States, please say so.", field: "Country" }
    ];

    for (const f of fields) {
        let attempt = 0;
        let answer = '';

        while (attempt < 2 && !answer) {
            const delay = f.field === "AddressLine1" || ZipCode ? 100 : 50;
            await speak(attempt === 0 ? f.question : "Please say it again. " + f.question, delay);
            await speak(attempt === 0 ? f.question : "Please say it again. " + f.question, 50);
            answer = await recognizeSpeech();
            attempt++;
        }

        if (answer) {
            let clean = answer;

            if (f.field === "Country") {
                const lower = clean.toLowerCase();
                if (!lower.includes("usa") && !lower.includes("united states")) {
                    await speak("You said a country other than the United States.");
                } else {
                    clean = "USA";
                }
            }

            console.log("📫 Address Field:", f.field, "→", clean);
            await dotNetHelper.invokeMethodAsync("SetSpeechInput", f.field, clean);
        }
    }

    await speak("Address section completed.");
};

function parseDate(input) {
    try {
        let cleaned = input.replace(/\b(\d+)(st|nd|rd|th)\b/gi, '$1').trim();
        cleaned = cleaned.replace(/[\s\-]/g, ' ');

        const now = new Date();
        const minDOB = new Date();
        minDOB.setFullYear(now.getFullYear() - 1);

        // Digit-only: 07011994 or 7011994
        const digitsOnly = cleaned.replace(/\D/g, '');
        if (digitsOnly.length === 8) {
            const mm = digitsOnly.slice(0, 2);
            const dd = digitsOnly.slice(2, 4);
            const yyyy = digitsOnly.slice(4);
            const parsed = new Date(`${mm}/${dd}/${yyyy} 12:00:00`);
            if (!isNaN(parsed) && parsed <= minDOB)
                return parsed.toISOString().split('T')[0];
        }

        // Spoken form like "July 1 1994"
        const parsed = new Date(cleaned + ' 12:00:00');
        if (!isNaN(parsed) && parsed <= minDOB)
            return parsed.toISOString().split('T')[0];

        // Fallback
        return input;
    } catch (e) {
        console.warn("❌ Date parse error:", e);
        return input;
    }
}
