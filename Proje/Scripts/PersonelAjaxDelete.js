$(function () {
    $("#tblPersonelGuncelle").on("click", ".deleteImage", function () {
        var id = $(this).data("id");
        $.ajax({
            type: "POST",
            url: "/Personel/ResimSil",
            data: { id: id },
            success: function () {
                var imagePreview = document.getElementById('imagePreview');
                imagePreview.src = '#';
                document.getElementById('ImageFile').value = '';
            },
            error: function (xhr, status, error) {
                console.error('Hata oluştu:', error);
                alert("Resim silinirken bir hata oluştu.");
            }
        });
    });
});