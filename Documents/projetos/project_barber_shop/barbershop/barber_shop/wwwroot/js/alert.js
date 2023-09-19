function openAlert() {
    Swal.fire({
        title: 'Voc� tem certeza que quer deletar este servi�o?',
        text: "Voc� n�o vai conseguir reverter isso!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: 'Cancelar',
        confirmButtonText: 'Sim, deletar!'
    }).then((result) => {
        if (result.isConfirmed) {
            return true;
        }
        return false;
    })
}