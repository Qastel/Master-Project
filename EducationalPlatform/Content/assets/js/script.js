$( document ).ready(function() {
    var w = window.innerWidth;
    
    if(w > 767){
        $('#menu-jk').scrollToFixed();
    }else{
        $('#menu-jk').scrollToFixed();
    }

    $(".form-textbox-fullname").blur(function () {
        ($(".form-textbox-fullname").val() === "") ? ($(".form-label-fullname").show() && $(".form-textbox-fullname").css("margin-top", "0px")) : ($(".form-label-fullname").hide() && $(".form-textbox-fullname").css("margin-top", "-23px"));
    });

    $(".form-textbox-fullname").focus(function () {
        //$(".form-label-fullname").show() && $(".form-textbox-fullname").css("margin-top", "0px");
    });

    $(".form-textbox-email").blur(function () {
        ($(".form-textbox-email").val() === "") ? ($(".form-label-email").show() && $(".form-textbox-email").css("margin-top", "0px")) : ($(".form-label-email").hide() && $(".form-textbox-email").css("margin-top", "-23px"));
    });

    $(".form-textbox-email").focus(function () {
        //$(".form-label-email").show() && $(".form-textbox-email").css("margin-top", "0px");
    });

    $(".form-textbox-psw").blur(function () {
        ($(".form-textbox-psw").val() === "") ? ($(".form-label-psw").show() && $(".form-textbox-psw").css("margin-top", "0px")) : ($(".form-label-psw").hide() && $(".form-textbox-psw").css("margin-top", "-23px"));
    });

    $(".form-textbox-psw").focus(function () {
       // $(".form-label-psw").show() && $(".form-textbox-psw").css("margin-top", "0px");
    });

    $(".form-textbox-repsw").blur(function () {
        ($(".form-textbox-repsw").val() === "") ? $(".form-label-repsw").show() && $(".form-textbox-repsw").css("margin-top", "0px") : ($(".form-label-repsw").hide() && $(".form-textbox-repsw").css("margin-top", "-23px"));
    });

    $(".form-textbox-repsw").focus(function () {
       // $(".form-label-repsw").show() && $(".form-textbox-repsw").css("margin-top", "0px");
    });

    $(".form-textbox-qualification").blur(function () {
        ($(".form-textbox-qualification").val() === "") ? ($(".form-label-qualification").show() && $(".form-textbox-qualification").css("margin-top", "0px")) : ($(".form-label-qualification").hide() && $(".form-textbox-qualification").css("margin-top", "-23px"));
    });

    $(".form-textbox-qualification").focus(function () {
      //  $(".form-label-qualification").show() && $(".form-textbox-qualification").css("margin-top", "0px");
    });

    
})

function showCompetenceTextBox() {
    $(".competence").show();
}   

function hideCompetenceTextBox() {
    $(".competence").hide();
}   