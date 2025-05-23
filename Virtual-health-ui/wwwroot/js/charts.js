// wwwroot/js/charts.js

window.vitalsCharts = {
    charts: {},

    /**
     * LINE CHART: Create a new line chart and store it under charts[canvasId].
     *
     * @param {string}   canvasId  ID of the <canvas> element
     * @param {string}   label     Dataset label
     * @param {string}   color     Border & fill color
     * @param {string[]} labels    Initial X-axis labels
     * @param {number[]} data      Initial Y-axis data
     * @returns {string|null}      The canvasId key, or null if failed
     */
    createStatic: function (canvasId, label, color, labels, data) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.error(`Canvas element not found: ${canvasId}`);
            return null;
        }
        canvas.style.backgroundColor = '#ffffff';
        const ctx = canvas.getContext('2d');

        const chart = new Chart(ctx, {
            type: 'line',
            data: {
                labels,
                datasets: [{
                    label,
                    data,
                    fill: true,
                    tension: 0.4,
                    backgroundColor: color + '33',
                    borderColor: color,
                    borderWidth: 2,
                    pointRadius: 4,
                    pointHoverRadius: 6
                }]
            },
            options: {
                maintainAspectRatio: false,
                animation: false,
                interaction: { mode: 'nearest', axis: 'x', intersect: false },
                plugins: {
                    legend: { display: false },
                    tooltip: { mode: 'index', intersect: false, backgroundColor: 'rgba(0,0,0,0.7)' }
                },
                scales: {
                    x: {
                        type: 'category',
                        grid: { display: false },
                        title: { display: true, text: 'Time' }
                    },
                    y: {
                        grid: { color: 'rgba(0,0,0,0.05)', drawBorder: false },
                        title: { display: true, text: label },
                        ticks: { stepSize: 1, callback: v => Number.isInteger(v) ? v : null }
                    }
                }
            }
        });

        this.charts[canvasId] = chart;
        return canvasId;
    },

    /**
     * LINE CHART: Update an existing line chart with new labels & data.
     *
     * @param {string}   canvasId
     * @param {string[]} labels
     * @param {number[]} data
     */
    update: function (canvasId, labels, data) {
        const chart = this.charts[canvasId];
        if (!chart) {
            console.error(`Chart not found for ${canvasId}`);
            return;
        }
        chart.data.labels = labels;
        chart.data.datasets[0].data = data;
        chart.update();
    },

    /**
     * BAR CHART: Create a grouped bar chart with dual Y-axes (y0 & y1)
     * and side-by-side bars.
     *
     * @param {string}   canvasId  ID of the <canvas>
     * @param {string[]} labels    X-axis labels (dates)
     * @param {object[]} datasets  Array of dataset objects:
     *   Each should include:
     *     - label
     *     - data (number[])
     *     - backgroundColor
     *     - borderColor
     *     - optional borderWidth
     *     - optional yAxisID ('y0' or 'y1')
     */
    createMultiBar: function (canvasId, labels, datasets) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.error(`Canvas element not found: ${canvasId}`);
            return null;
        }
        canvas.style.backgroundColor = '#ffffff';
        const ctx = canvas.getContext('2d');

        // Auto-assign yAxisID if missing, and set bar sizing/order
        datasets.forEach((ds, i) => {
            ds.yAxisID = ds.yAxisID || (i === 0 ? 'y0' : 'y1');
            ds.barPercentage = 0.5;
            ds.categoryPercentage = 0.8;
            ds.order = i === 0 ? 1 : 2;
        });

        const chart = new Chart(ctx, {
            type: 'bar',
            data: { labels, datasets },
            options: {
                maintainAspectRatio: false,
                scales: {
                    x: {
                        title: { display: true, text: 'Date' },
                        grid: { display: false }
                    },
                    y0: {
                        type: 'linear',
                        position: 'left',
                        title: { display: true, text: 'Duration (h)' },
                        beginAtZero: true,
                        ticks: { stepSize: 1 }
                    },
                    y1: {
                        type: 'linear',
                        position: 'right',
                        title: { display: true, text: 'Quality (%)' },
                        beginAtZero: true,
                        max: 100,
                        grid: { drawOnChartArea: false },
                        ticks: { stepSize: 10 }
                    }
                },
                plugins: {
                    legend: { position: 'top' },
                    tooltip: { mode: 'index', intersect: false }
                }
            }
        });

        this.charts[canvasId] = chart;
        return canvasId;
    },

    /**
     * BAR CHART: Update an existing grouped bar chart.
     *
     * @param {string}   canvasId
     * @param {string[]} labels
     * @param {object[]} datasets
     */
    updateMultiBar: function (canvasId, labels, datasets) {
        const chart = this.charts[canvasId];
        if (!chart) {
            console.error(`Chart not found for ${canvasId}`);
            return;
        }

        // Repeat sizing/order logic on update
        datasets.forEach((ds, i) => {
            ds.yAxisID = ds.yAxisID || (i === 0 ? 'y0' : 'y1');
            ds.barPercentage = 0.5;
            ds.categoryPercentage = 0.8;
            ds.order = i === 0 ? 1 : 2;
        });

        chart.data.labels = labels;
        chart.data.datasets = datasets;
        chart.update();
    }
};
