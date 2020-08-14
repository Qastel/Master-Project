$( document ).ready(function() {
    var w = window.innerWidth;
    

    if(w > 767){
        $('#menu-jk').scrollToFixed();
    }else{
        $('#menu-jk').scrollToFixed();
    }

    $(".form-textbox-fullname").blur(function () {
        ($(".form-textbox-fullname").val() === "") ? $(".form-label-fullname").show() : $(".form-label-fullname").hide();
    });

    $(".form-textbox-fullname").focus(function () {
        $(".form-label-fullname").show();
    });

    $(".form-textbox-email").blur(function () {
        ($(".form-textbox-email").val() === "") ? $(".form-label-email").show() : $(".form-label-email").hide();
    });

    $(".form-textbox-email").focus(function () {
        $(".form-label-email").show();
    });

    $(".form-textbox-psw").blur(function () {
        ($(".form-textbox-psw").val() === "") ? $(".form-label-psw").show() : $(".form-label-psw").hide();
    });

    $(".form-textbox-psw").focus(function () {
        $(".form-label-psw").show();
    });

    $(".form-textbox-repsw").blur(function () {
        ($(".form-textbox-repsw").val() === "") ? $(".form-label-repsw").show() : $(".form-label-repsw").hide();
    });

    $(".form-textbox-repsw").focus(function () {
        $(".form-label-repsw").show();
    });
    
})
