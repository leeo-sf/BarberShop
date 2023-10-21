function removeCaracteres() {
    const cpf = document.querySelector("#cpf");

    let value = cpf.value.replace(/[^0-9]/g, "");

    cpf.value = value;
}

function BuscaCep() {
    var zipCodeUser = document.querySelector('#zipCode').value;
    $.ajax({
        type: "GET",
        url: "GetAddressData",
        data: { zipCode: zipCodeUser },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.cep === null) {
                alert('CEP não encontrado');
            }
            $('#publicPlace').val(data.logradouro);
            $('#neighborhood').val(data.bairro);
            $('#locality').val(data.localidade);
        }
    })
}

function capturaValor(element) {
    $(document).ready(function () {
        $("#valorSelecionado").text(element).val();
    })
}

function AlterouForm() {
    var btn = document.getElementById("reagendar");
    btn.disabled = false;
    btn.style = "opacity: none";
}

function formatCpf() {
    const cpf = document.querySelector("#cpf");
    cpf.value = cpf.value.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
}

function DesabilitaHorarioDomingo() {
    var dateScheduling2 = document.getElementById("dateScheduling").value;
    var dataConvertida = new Date(dateScheduling2);

    if (dataConvertida.getDay() === 6) {
        $("#8").prop('disabled', true);
        $("#9").prop('disabled', true);
        $("#10").prop('disabled', true);
        $("#11").prop('disabled', true);
        $("#12").prop('disabled', true);
        $("#13").prop('disabled', true);
        $("#14").prop('disabled', true);
        $("#15").prop('disabled', true);
        $("#16").prop('disabled', true);
    } else {
        $("#8").prop('disabled', false);
        $("#9").prop('disabled', false);
        $("#10").prop('disabled', false);
        $("#11").prop('disabled', false);
        $("#12").prop('disabled', false);
        $("#13").prop('disabled', false);
        $("#14").prop('disabled', false);
        $("#15").prop('disabled', false);
        $("#16").prop('disabled', false);
    }
}