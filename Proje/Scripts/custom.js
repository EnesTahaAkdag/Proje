
$(function () {
    $("#tblDepartmanlar").DataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
        }
    });

});
$(function () {
    $("#tblDepartmanlar").on("click", ".btnDepartmanSil", function () {
        var btn = $(this);
        var departmanId = btn.data("id");

        $.ajax({
            url: "/Departman/Personelvarmi/",
            dataType: 'json',
            type: "POST",
            cache: false,
            data: { departmanId: departmanId },
        }).done(function (result) {
            if (result) {
                bootbox.dialog({
                    title: "Departmanı Sil",
                    message: "Departmanı Silmek İstediğinize Emin misiniz?",
                    buttons: {
                        cancel: {
                            label: 'İptal',
                            className: 'btn-primary'
                        },
                        confirm: {
                            label: 'Departman ve Personeli sil',
                            className: 'btn-warning',
                            callback: function (result) {
                                $.ajax({
                                    url: "/Departman/Sil/",
                                    type: "POST",
                                    cache: false,
                                    data: { departmanId: departmanId, type: "removeDptandPersonel" },
                                    success: function () {
                                        btn.parent().parent().fadeOut(400);
                                    }
                                });
                            }
                        },
                        noclose: {
                            label: 'Departmanı sil ve Personeli boşa çıkar',
                            className: 'btn-danger',
                            callback: function (result) {
                                $.ajax({
                                    url: "/Departman/Sil/",
                                    type: "POST",
                                    cache: false,
                                    data: { departmanId: departmanId, type: "removeDptemptyPersonel" },
                                    success: function () {
                                        btn.parent().parent().remove();
                                    }
                                });
                            }
                        }
                    },
                });
            }
            else {
                bootbox.dialog({
                    title: "Boş Departmanı Sil",
                    message: "Boş Departmanı Silmek İstediğinize Emin misiniz?",
                    buttons: {
                        cancel: {
                            label: 'İptal',
                            className: 'btn-primary'
                        },
                        confirm: {
                            label: 'Departman Sil',
                            className: 'btn-warning',
                            callback: function (result) {
                                $.ajax({
                                    url: "/Departman/Sil/",
                                    type: "POST",
                                    cache: false,
                                    data: { departmanId: departmanId, type: "onlyDpt" },
                                    success: function () {
                                        btn.parent().parent().fadeOut(400);
                                    }
                                })
                            }
                        }
                    }
                });
            }
        });
    });
});
$(function () {
    $("#tblPersoneller").on("click", ".btnPersonelSil", function () {
        var btn = $(this);
        var personelId = btn.data("id");
        bootbox.dialog({
            title: "Personel Sil",
            message: "Personeli Silmek İstediğinize Emin misiniz?",
            buttons: {
                cancel: {
                    label: 'İptal',
                    className: 'btn-primary'
                },
                confirm: {
                    label: 'Personel Sil',
                    className: 'btn-warning',
                    callback: function (result) {
                        $.ajax({
                            type: "POST",
                            url: "/Personel/Sil/" + personelId,
                            data: { personelId: personelId},
                            success: function () {
                                btn.parent().parent().fadeOut(400);
                            }
                        })
                    }
                }
            }
        });
    });
});