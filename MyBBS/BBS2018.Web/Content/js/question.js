$(function () {

    //提问
    $('.question').on('click', function (e) {

        var askUrl = $('input[name="askquesUrl"]').val();
        $.get(askUrl, function (data) {

            layer.open({
                type: 1,
                title: false,
                closeBtn: 2,
                area: ['450px', '200px'],
                skin: 'ask-qu',
                content: data,
                btn: ['发布问题'],
                btnAlign: 'c',
                yes: function (index, layero) {

                    var content = $('.qu-content').val();
                    if ($.trim(content) == '') {
                        $('.qu-tips .tips-text').text('问题不能为空');
                        $('.qu-tips').show();
                        return;
                    }

                    var saveUrl = $('input[name="saveAskUrl"]').val();
                    $.ajax({
                        type: 'post',
                        url: saveUrl,
                        data: JSON.stringify({ Title: $('.qu-content').val() }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (data) {
                        
                            layer.close(index);
                        }
                    });
                },
                success: function (layero, index) {

                }
            });

            //隐藏提示
            $('.qu-content').on('focus', function () {

                $('.qu-tips').hide();
            });
        });
    });


    function getQuestions() {



    }
});