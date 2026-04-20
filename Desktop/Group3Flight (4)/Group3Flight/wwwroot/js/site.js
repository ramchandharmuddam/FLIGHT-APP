// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    let selectedDate = $('#departureDate').val();
    $('#departureDate').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        minDate: moment(),
        startDate: selectedDate
            ? moment(selectedDate)
            : moment().add(1, 'days'),
        locale: {
            format: 'YYYY-MM-DD'
        }
    });

});
