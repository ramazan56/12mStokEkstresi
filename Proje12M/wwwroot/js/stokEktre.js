function stokEktresiGetir() {

    var data = GetFormData($("#dataForm"));
    if (data.bitTarih == '' || data.basTarih == '' || data.malAdi == 'Mal Adı Seçiniz.') {
        alert("Lütfen Listeyi Görmek İçin Tarih seçin ve listeden MalAdı Seçin")
    }
    else {
        $.ajax({
            url: 'StokEkstre/StokEktresi',
            type: 'GET',
            data: { 'basTarih': data.basTarih, 'bitTarih': data.bitTarih, 'malAdi': data.malAdi },
            dataType: 'json',
            success: function (result) {
                $('#gridContainer').dxDataGrid({
                    dataSource: result,
                    keyExpr: 'id',
                    columns: ['siraNo', 'evrakNo', 'id', 'islemTur', 'miktar', 'tarih', 'girisMiktar', 'cikisMiktar', 'stokMiktar'],
                    showBorders: true,
                    export: {
                        enabled: true
                    },
                    onExporting: function (e) {
                        var workbook = new ExcelJS.Workbook();
                        var worksheet = workbook.addWorksheet('Main sheet');
                        DevExpress.excelExporter.exportDataGrid({
                            worksheet: worksheet,
                            component: e.component,
                            customizeCell: function (options) {
                                var excelCell = options;
                                excelCell.font = { name: 'Arial', size: 12 };
                                excelCell.alignment = { horizontal: 'left' };
                            }
                        }).then(function () {
                            workbook.xlsx.writeBuffer().then(function (buffer) {
                                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
                            });
                        });
                        e.cancel = true;
                    }
                   
                });

            },
            error: function (error) {

            }
        });
        var dataGrid = $("#gridContainer").dxDataGrid({
            // ...
        }).dxDataGrid("instance");
        var dataSource = dataGrid.getDataSource();
        dataSource.store().insert(data).then(function () {
            dataSource.reload();
        })
    }

}
function GetFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });
    console.log(indexed_array)
    return indexed_array;
}


