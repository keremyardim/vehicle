// Write your JavaScript code.
$(document).ready(function () {
    $("#VehicleBrandID").on("change", function (e) {
        var drpElment = this;
        var selectedId = $(drpElment).find(":selected").val();
        if (selectedId) {
            $.get(
                "/vehicle/GetModelOfBrand",
                { id: selectedId },
                function (data) {
                    if (data) {
                        $("#VehicleModelID").find('option').remove();
                        $.each(data, function (index, elm) {
                            console.log(elm);

                            $("#VehicleModelID").append($('<option>',
                                {
                                    value: elm.value,
                                    text: elm.text,
                                    selected: elm.selected
                                }));
                        });
                    }

                }
            );
        }
    });
    $("#chkState").on("change", function (e) {
        if ($("#chkState").prop('checked') == true) {
            $("#State").val(1);
        } else {
            $("#State").val(0);
        }
    })
})
