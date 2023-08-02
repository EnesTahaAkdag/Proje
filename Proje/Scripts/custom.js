
$(function () {
    $("#tblDepartmanlar").DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
        }
    });

    $("#tblDepartmanlar").on("click", ".btnDepartmanSil", function () {

        var btn = $(this);

        bootbox.dialog({
            message: "Custom dialog using Bootbox",
            title: "Departmanı Sil",
            buttons: {
                success: {
                    label: "Departmanı sil",
                    className: "btn-danger",
                    callback: function () {
                        alert("Departman Silindi Personeller Boşa Çıktı Personellere Departmana Atayınız");
                    }
                },
                danger: {
                    label: "Departan ve Personelleri Sil",
                    className: "btn-danger",
                    callback: function (result) {


                        if (result) {

                            var id = btn.data("id");
                            $.ajax({
                                type: "GET",
                                url: "/Departman/Sil/" + id,
                                success: function () {
                                    btn.parent().parent().remove();
                                }
                            });
                            alert("Departman ve Personeller Silindi");
                        }
                    },
                    main: {
                        label: "Silme İşlemini İpatal Et",
                        className: "btn-success",
                        callback: function () {
                            alert("Silme İşlemi İptal edildi");

                        }
                    }
                }
    }

