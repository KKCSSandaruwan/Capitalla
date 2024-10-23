function errorProjectUpload() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, Please upload a picture to continue!!!',
        showConfirmButton: false,
        timer: 1200
    })
}
function success() {
    Swal.fire({
        position: 'center',
        icon: 'success',
        title: 'Yes, Save Successfully',
        showConfirmButton: false,
        timer: 1200
    })
}
function update() {
    Swal.fire({
        position: 'center',
        icon: 'success',
        title: 'Yes, Update Successfully',
        showConfirmButton: false,
        timer: 1200
    })
}
function delete1() {
    Swal.fire({
        position: 'center',
        icon: 'success',
        title: 'Yes, Delete Successfully',
        showConfirmButton: false,
        timer: 1200
    })
}
function usedata() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, Cannot be delete. already in used.',
        showConfirmButton: false,
        timer: 1200
    })
}
function error() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, Something is wrong',
        showConfirmButton: false,
        timer: 1200
    })
}
function errorUserExist() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, already exists.',
        showConfirmButton: false,
        timer: 1200
    })
}
function errorInvalidTimeSheet() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, Invalid Date Time!!!',
        showConfirmButton: false,
        timer: 1200
    })
}
function errorNotTime() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: "Opp, You Can't Select Date You Haven't Worked Yet !!!",
        showConfirmButton: false,
        timer: 1500
    })
}

function errorFileMissing() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, You forgot to upload a file',
        showConfirmButton: false,
        timer: 1200
    })
}
function errorPasswordNotMatch() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, Password and Confirm Password not matched',
        showConfirmButton: false,
        timer: 1200
    })
}
function errorMissingRole() {
    Swal.fire({
        position: 'center',
        icon: 'error',
        title: 'Opp, You forgot to choose role for this user',
        showConfirmButton: false,
        timer: 1200
    })
}
