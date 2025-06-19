// ✅ Check voice support
window.supportsVoice = ('SpeechRecognition' in window) || ('webkitSpeechRecognition' in window);

// ✅ Voice assistant banner toggle
window.toggleVoiceBanner = function (isActive) {
    let banner = document.getElementById("voiceBanner");

    if (isActive) {
        if (!banner) {
            banner = document.createElement("div");
            banner.id = "voiceBanner";
            banner.style.position = "fixed";
            banner.style.top = "0";
            banner.style.width = "100%";
            banner.style.padding = "12px";
            banner.style.backgroundColor = "#198754";
            banner.style.color = "#fff";
            banner.style.textAlign = "center";
            banner.style.zIndex = "99999";
            banner.style.fontWeight = "bold";
            banner.style.cursor = "pointer";
            banner.innerText = "🎙 Voice Assistant Active — Click to Stop";

            banner.onclick = () => window.stopVoiceAssistant();
            document.body.appendChild(banner);
        }
    } else {
        if (banner) banner.remove();
    }
};

// ✅ Speak helper
function speak(text, delayAfterSpeech = 100) {
    return new Promise(resolve => {
        const msg = new SpeechSynthesisUtterance(text);
        msg.lang = "en-US";
        msg.onend = () => setTimeout(resolve, delayAfterSpeech);
        speechSynthesis.cancel();
        speechSynthesis.speak(msg);
    });
}

// ✅ Speech recognition helper
async function recognizeSpeech() {
    return new Promise(resolve => {
        const recognition = new (window.SpeechRecognition || window.webkitSpeechRecognition)();
        recognition.lang = 'en-US';
        recognition.interimResults = false;
        recognition.maxAlternatives = 1;

        recognition.onresult = (event) => {
            const result = event.results[0][0].transcript.trim();
            console.log("🎤 Raw Speech Input:", result);
            document.getElementById("speechLog")?.insertAdjacentHTML('beforeend', `<div>🗣️ ${result}</div>`);
            if (result.toLowerCase().includes("stop voice assistant")) {
                window.stopVoiceAssistant();
                resolve('');
            } else {
                resolve(result);
            }
        };

        recognition.onerror = (event) => {
            console.warn("❌ Speech error:", event.error);
            speak("I didn’t catch that. Please try again.");
            resolve('');
        };

        if (speechSynthesis.speaking) {
            const interval = setInterval(() => {
                if (!speechSynthesis.speaking) {
                    clearInterval(interval);
                    recognition.start();
                }
            }, 100);
        } else {
            recognition.start();
        }
    });
}

// ✅ Parse and validate date of birth
function parseDate(input) {
    try {
        let cleaned = input.replace(/\b(\d+)(st|nd|rd|th)\b/gi, '$1').trim();
        cleaned = cleaned.replace(/[\s\-]/g, ' ');
        const digitsOnly = cleaned.replace(/\D/g, '');

        if (digitsOnly.length === 8) {
            const mm = digitsOnly.slice(0, 2);
            const dd = digitsOnly.slice(2, 4);
            const yyyy = digitsOnly.slice(4);
            const parsed = new Date(`${mm}/${dd}/${yyyy} 12:00:00`);
            if (isValidDob(parsed)) return parsed.toISOString().split('T')[0];
        }

        const spaced = cleaned.split(' ');
        if (spaced.length === 2 && spaced[0].length === 3) {
            const mm = spaced[0][0];
            const dd = spaced[0].slice(1);
            const yyyy = spaced[1];
            const parsed = new Date(`${mm}/${dd}/${yyyy} 12:00:00`);
            if (isValidDob(parsed)) return parsed.toISOString().split('T')[0];
        }

        const parsed = new Date(cleaned + ' 12:00:00');
        if (isValidDob(parsed)) return parsed.toISOString().split('T')[0];

        return "";
    } catch (e) {
        console.warn("❌ Date parse error:", e);
        return "";
    }
}

function isValidDob(date) {
    if (!(date instanceof Date) || isNaN(date)) return false;
    const today = new Date();
    const oneYearAgo = new Date(today);
    oneYearAgo.setFullYear(today.getFullYear() - 1);
    return date < oneYearAgo;
}

// ✅ Open accordion
window.openAccordionSection = function (sectionId) {
    const all = document.querySelectorAll('.accordion-collapse');
    all.forEach(sec => sec.classList.remove('show'));
    const target = document.getElementById(sectionId);
    if (target) target.classList.add('show');
};

// ✅ Start voice assistant
window.startVoiceAssistant = async function (dotNetHelper) {
    if (!window.supportsVoice) {
        alert("Voice recognition is not supported in this browser.");
        return;
    }
    window.toggleVoiceBanner(true);
    await startVoiceAssistantIntro(dotNetHelper);
};

// ✅ Stop voice assistant
window.stopVoiceAssistant = async function () {
    speechSynthesis.cancel();
    window.toggleVoiceBanner(false);
    await speak("Voice assistant has been stopped.");
};

// ✅ Personal Details
window.startVoiceAssistantIntro = async function (dotNetHelper) {
    await speak("Let's fill your personal details.");
    window.openAccordionSection("collapsePersonal");

    const fields = [
        { question: "What is your first name?", field: "FirstName", delay: 100 },
        { question: "What is your last name?", field: "LastName" },
        { question: "What is your date of birth? For example, say January first 2000 or zero one zero one 2000.", field: "BirthDate", isDate: true },
        { question: "What is your gender? Say male, female, other, or unknown.", field: "Gender" },
        { question: "What is your phone number?", field: "PhoneNumber" }
    ];

    for (const f of fields) {
        let attempt = 0, answer = '';
        while (attempt < 2 && !answer) {
            await speak(attempt === 0 ? f.question : "Please say again. " + f.question, f.delay || 50);
            answer = (await recognizeSpeech()).trim();
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

            if (f.field === "BirthDate" && !clean) {
                console.warn("Invalid DOB — fallback to 2000-01-01");
                clean = "2000-01-01";
            }

            console.log("📝 Field:", f.field, "→", clean);
            await dotNetHelper.invokeMethodAsync("SetSpeechInput", f.field, clean);
        }
    }

    await speak("Done filling personal details. Let's fill the address now.");
    await window.startVoiceAssistantAddress(dotNetHelper);
};

// ✅ Address Details
window.startVoiceAssistantAddress = async function (dotNetHelper) {
    await speak("Now let's fill your address details.");
    window.openAccordionSection("collapseAddress");

    const fields = [
        { question: "What is your address line one?", field: "AddressLine1", delay: 100 },
        { question: "What is your street name?", field: "Street" },
        { question: "Which city do you live in?", field: "City" },
        { question: "What is your state?", field: "State" },
        { question: "What is your zip code?", field: "ZipCode" },
        { question: "What is your country? If it's not the United States, please say so.", field: "Country" }
    ];

    for (const f of fields) {
        let attempt = 0, answer = '';
        while (attempt < 2 && !answer) {
            await speak(attempt === 0 ? f.question : "Please say again. " + f.question, f.delay || 50);
            answer = (await recognizeSpeech()).trim();
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