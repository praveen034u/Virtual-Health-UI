window.speechToText = function (dotNetHelper, fieldName, isDate) {
    if (!('webkitSpeechRecognition' in window) && !('SpeechRecognition' in window)) {
        alert('Speech recognition not supported in this browser.');
        return;
    }
    const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
    const recognition = new SpeechRecognition();
    recognition.lang = 'en-US';
    recognition.interimResults = false;
    recognition.maxAlternatives = 1;

    recognition.onresult = function (event) {
        let transcript = event.results[0][0].transcript.trim();
        if (isDate) {
            transcript = transcript.replace(/(st|nd|rd|th)/gi, '');
            const date = new Date(transcript);
            if (!isNaN(date)) {
                transcript = date.toISOString().split('T')[0];
            }
        } else if (fieldName === "Gender") {
            const lower = transcript.toLowerCase();
            if (lower.includes("male")) transcript = "male";
            else if (lower.includes("female")) transcript = "female";
            else if (lower.includes("other")) transcript = "other";
            else if (lower.includes("unknown")) transcript = "unknown";
        }
        dotNetHelper.invokeMethodAsync('SetSpeechInput', fieldName, transcript);
    };

    recognition.onerror = function (event) {
        alert('Speech recognition error: ' + event.error);
    };

    recognition.start();
};