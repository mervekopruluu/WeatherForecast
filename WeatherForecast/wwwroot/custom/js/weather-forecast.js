
$('#tbWeatherForecast').dataTable({
    paging: false,
    searching: false
});

$("#btnWeather").on("click", function () {
    if ($('#cities').val() == 0) {
        swal("City ​is required!", {
            icon: "danger",
        });
    }
    else if ($('#date').val() == "") {
        swal("Date ​is required!", {
            icon: "danger",
        });
    }
    else {
        $.get("/Home/Get", { city: $('#cities').val(), date: $('#date').val() }, function (data, status) {
            $('#tbWeatherForecast').dataTable().fnDestroy();

            $('#tbWeatherForecast').dataTable({
                paging: false,
                searching: false,
                dom: '<"row"<"col-lg-6"l><"col-lg-6"f>><"table-responsive"t>p',
                data: data,
                columns: [

                    {
                        "data": "Date",
                        "render": function (data, type, full, meta) {
                            return moment(full.date).format('DD/MM/YYYY')
                        }
                    },
                    {
                        "data": "MinDegree",
                        "render": function (data, type, full, meta) {
                            return parseInt(full.minDegree) + " °C "
                        }
                    },
                    {
                        "render": function (data, type, full, meta) {
                            return parseInt(full.maxDegree) + " °C "
                        }
                    }

                ],
                columnDefs: [
                    {
                        "targets": 1,
                        "className": "text-center",
                    }
                ],

            });
        });
    }

})