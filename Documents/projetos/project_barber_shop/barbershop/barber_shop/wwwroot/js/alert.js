function openAlert() {
    Swal.fire({
        title: 'Você tem certeza que quer deletar este serviço?',
        text: "Você não vai conseguir reverter isso!",
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