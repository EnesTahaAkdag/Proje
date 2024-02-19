$(function () {
    $("#tblPersonelGuncelle").on("click", ".deleteImage", function () {
        var id = $(this).data("id");
        debugger
        $.ajax({
            
            type: "POST",
            url: "/Personel/ResimSil",
            data: { id: id}, 
            error: function (xhr, status, error) {
                console.error('Hata oluştu:', error);
                alert("Resim silinirken bir hata oluştu.");
            }
        });
    });
});
