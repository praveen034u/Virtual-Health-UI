window.CalendarExport = {
    downloadICS: (eventTitle, description, startUTC, endUTC) => {
        const ics = `
BEGIN:VCALENDAR
VERSION:2.0
BEGIN:VEVENT
SUMMARY:${eventTitle}
DESCRIPTION:${description}
DTSTART:${startUTC}
DTEND:${endUTC}
END:VEVENT
END:VCALENDAR`.trim();

        const blob = new Blob([ics], { type: "text/calendar;charset=utf-8" });
        const a = document.createElement("a");
        a.href = URL.createObjectURL(blob);
        a.download = `${eventTitle.replace(/\s+/g, "_")}.ics`;
        a.click();
    }
};
