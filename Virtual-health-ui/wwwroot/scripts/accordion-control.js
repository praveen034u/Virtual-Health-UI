window.toggleAllAccordions = function (expand) {
    const sections = document.querySelectorAll('.accordion-collapse');
    sections.forEach(section => {
        if (expand) {
            section.classList.add('show');
        } else {
            section.classList.remove('show');
        }
    });
};
window.openAccordionSection = function (sectionId) {
    const all = document.querySelectorAll('.accordion-collapse');
    all.forEach(a => a.classList.remove('show')); // Collapse others

    const target = document.getElementById(sectionId);
    if (target) {
        target.classList.add('show');
    }
};
