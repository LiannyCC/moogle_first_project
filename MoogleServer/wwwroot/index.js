const body = document.querySelector("body")
let isLoad = false;
// para controlar el movimiento de los ojos en el logo
body.addEventListener("mousemove", () => {
    var eye = document.querySelectorAll(".eye");

    eye.forEach(function (eye) {
        let x = eye.getBoundingClientRect().left + eye.clientWidth / 2;
        let y = eye.getBoundingClientRect().top + eye.clientHeight / 2;
        let radian = Math.atan2(event.pageX - x, event.pageY - y);
        let rot = radian * (180 / Math.PI) * -1 + 270;

        eye.style.transform = "rotate(" + rot + "deg)";
    });
});


function clickHandler() {
    const query = document.querySelector("#query")
    const load = document.querySelector("#wifi-loader")
    const listResults = document.querySelector("#list-results")
    const cantResults = document.querySelector("#cant-results")
    const suggestion = document.querySelector("#suggestion")
    
    if (!isLoad) {
        load.style.display = "flex"
        listResults.style.display = "none"
        cantResults.style.display = "none"
        suggestion.style.display = "none"
    }
    else { 
        load.style.display = "none"
        listResults.style.display = "block"
        cantResults.style.display = "block"
        suggestion.style.display = "block"
    }

    isLoad = !isLoad
}
 
function launchClick() { 
    click = document.querySelector("#launch-click") 

    clickHandler()

    console.log(click)
} 

function DropdownQuery(isFocus) {
    const dropdownQuery = document.querySelector("#dropdown-query")
    
    if (isFocus) {
        dropdownQuery.style.position = "absolute";
        dropdownQuery.style.inset =  "0px auto auto 0px";
        dropdownQuery.style.margin = "0px";
        dropdownQuery.style.transform = "translate3d(0px, 56px, 0px)";
        dropdownQuery.style.display = "block"
    }
    else {
        dropdownQuery.style.position = "none";
        dropdownQuery.style.inset =  "none";
        dropdownQuery.style.margin = "none";
        dropdownQuery.style.transform = "none";
        dropdownQuery.style.display = "none"
    }
    
}