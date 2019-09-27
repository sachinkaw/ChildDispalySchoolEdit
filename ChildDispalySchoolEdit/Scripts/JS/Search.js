$(function () {
        $("#updbtn").click(function () {
            var schId = $('#SchoolNames').val();
            var chdID = $('#chdId').val();
            alert(schId);
            alert(chdID);
            $.post("/Home/UpdateSchool", { schId: schId, chdID: chdID }, function (data) { alert('Success') });
        });
    });