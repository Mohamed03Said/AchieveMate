function toggleSidebar() {
    const sidebar = document.getElementById('sidebar');
    const content = document.getElementById('content');
    sidebar.classList.toggle('sidebar-open');
    content.classList.toggle('shifted');
}

const today = new Date();
const dayNames = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
const day = dayNames[today.getDay()];
const date = today.toLocaleDateString();

document.getElementById('currentDay').innerText = day;
document.getElementById('currentDate').innerText = date;

window.onload = function () {
    let lastCheckedDate = new Date();

    function checkForNewDay() {
        const currentDate = new Date();

        if (currentDate.toDateString() !== lastCheckedDate.toDateString()) {
            console.log("new day....")
            window.location.reload();
        }

        lastCheckedDate = currentDate;
    }

    setInterval(checkForNewDay, 60 * 1000);
};