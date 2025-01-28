
var passwordBox = document.getElementById("password");
passwordBox.addEventListener("click", function () {
    passwordBox.disabled = false;
});

var nameBox = document.getElementById("name");
nameBox.addEventListener("click", function () {
    nameBox.disabled = false;
});

var radio = document.getElementById("radio");
var submit = document.getElementById("submit");
submit.style.backgroundColor = "grey";
radio.addEventListener("click", function () {
    submit.style.backgroundColor = "black";
    submit.disabled = false;
});

var popUp = document.getElementById("popUp");
function hidePopUp() {
   popUp.style.display = 'none';
}
setTimeout(hidePopUp, 8000);

var closePopup = document.getElementById("close");
closePopup.addEventListener("click", hidePopUp());

var video = document.getElementById('video');
video.addEventListener('mouseover', function () {
    video.play();
});

video.addEventListener('mouseout', function () {
    video.pause();
    video.currentTime = 0;
});

function previewImage(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('profile-image');
        output.src = reader.result;
    };
    reader.readAsDataURL(event.target.files[0]);
}

var editName = document.getElementById("editName");
editName.addEventListener("click", function () {
    document.getElementById("name").disabled = false;
});

var editEmail = document.getElementById("editEmail");
editEmail.addEventListener("click", function () {
    document.getElementById("email").disabled = false;
});

var editPassword = document.getElementById("editPassword");
editPassword.addEventListener("click", function () {
    document.getElementById("password").disabled = true;
});

function ul(index) {
    console.log('click!' + index)

    var underlines = document.querySelectorAll(".underline");

    for (var i = 0; i < underlines.length; i++) {
        underlines[i].style.transform = 'translate3d(' + index * 100 + '%,0,0)';
    }
}

function previewImage(event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('profile-image');
        output.src = reader.result;
    };
    reader.readAsDataURL(event.target.files[0]);
}

    var confirmation = document.getElementById("delConfirmation");
    var btnDelete = document.getElementById("delete");
    btnDelete.addEventListener("click", function () {
        confirmation.style.display = 'block';
        });

    var noDelete = document.getElementById("NoDelete");
    var delConfirmation = document.getElementById("delConfirmation");
    noDelete.addEventListener("click", function () {
        delConfirmation.style.display = 'none';
        });

    function showPopup() {
            var popup = document.getElementById('popup-account');
    popup.style.display = 'block';
        }

    window.onload = function () {
        showPopup();
        };

    function closePopup() {
            var popup = document.getElementById('popup-account');
    popup.style.display = 'none';
        }