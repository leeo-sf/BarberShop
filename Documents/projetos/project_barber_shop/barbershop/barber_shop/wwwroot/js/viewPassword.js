document.getElementById('eye').addEventListener('mousedown', function () {
    document.getElementById('password').type = 'text';
});

document.getElementById('eye').addEventListener('mouseup', function () {
    document.getElementById('password').type = 'password';
});

document.getElementById('eye').addEventListener('mousemove', function () {
    document.getElementById('password').type = 'password';
});