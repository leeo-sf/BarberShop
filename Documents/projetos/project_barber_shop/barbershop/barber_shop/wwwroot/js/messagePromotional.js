function InputTitleEmail() {
    let txtDigitadoTitle = document.getElementById("inputTitleEmail").value;
    let txtPreViewTitle = document.getElementById("titleEmail");

    if (txtDigitadoTitle === "") {
        txtDigitadoTitle = "Título do Email";
    }

    txtPreViewTitle.innerText = txtDigitadoTitle;
}

function InputEmailBody() {
    let txtDigitadoBody = document.getElementById("inputEmailBody").value;
    let txtPreViewBody = document.getElementById("emailBody");

    if (txtDigitadoBody === "") {
        txtDigitadoBody = "Corpo do Email";
    }

    txtPreViewBody.innerText = txtDigitadoBody;
}

function ConfirmarEnvio() {
    if (confirm("Tem certeza que deseja enviar esse email a todos os clientes cadastrados?") == true) {
        return true;
    } else {
        return false;
    }
}