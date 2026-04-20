jQuery.validator.addMethod("validdate", function (value, element, param) {

    if (!value) return false;

    var parts = value.split("-");
    if (parts.length !== 3) return false;

    var parsedDate = new Date(parts[0], parts[1] - 1, parts[2]);
    if (isNaN(parsedDate.getTime())) return false;

    var yearsLimit = parseInt(param, 10);

    var currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0);

    var upperLimitDate = new Date(currentDate);
    upperLimitDate.setFullYear(upperLimitDate.getFullYear() + yearsLimit);

    return parsedDate > currentDate && parsedDate <= upperLimitDate;
});
jQuery.validator.unobtrusive.adapters.addSingleVal("validdate", "years");