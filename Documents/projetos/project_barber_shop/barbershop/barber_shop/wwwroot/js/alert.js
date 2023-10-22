function Confirmar() {
    if (confirm("Deseja deletar? Esta acao nao podera ser desfeita.") == true) {
        return true;
    } else {
        return false;
    }
}