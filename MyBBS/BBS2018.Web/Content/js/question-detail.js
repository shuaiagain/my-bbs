$(function () {

    $('.write-an').on('click', function (e) {

        var writeUrl = $('input[name="writeAnswerUrl"]').val();
        $.get(writeUrl, function (data) {

            layer.open({
                type: 1,
                title: false,
                closeBtn: 2,
                area: ['650px', '370px'],
                skin: 'write-an',
                content: data,
                btn: ['提交回答'],
                btnAlign: 'r',
                yes: function (index, layero) {

                    var content = $('.an-content').val();
                    if ($.trim(content) == '') {
                        $('.qu-tips .tips-text').text('回答不能为空');
                        $('.qu-tips').show();
                        return;
                    }

                    var saveUrl = $('input[name="saveAnswerUrl"]').val();
                    $.ajax({
                        type: 'post',
                        url: saveUrl,
                        data: JSON.stringify({ Content: $('.an-content').val(), QuestionID: $('input[name="questionId"]').val() }),
                        contentType: 'application/json;charset=utf-8',
                        success: function (data) {

                            layer.close(index);
                        }
                    });
                },
                success: function (layero, index) {

                }
            });
        });
    });

    function getAnswerList() {

        var getQuesUrl = $('input[name="getQuesUrl"]').val();
        var arguments = {
            PageIndex: pageIndex ? 1 : pageIndex,
            PageSize: pageSize ? 10 : pageSize,
            KeyWord: keyWord
        };

        $.ajax({
            type: 'post',
            url: getQuesUrl,
            data: JSON.stringify(arguments),
            contentType: 'application/json;charset=utf-8',
            success: function (data) {

                if (data.Code == -200) {
                    $('.main-content').html(renderHtml());
                    return;
                }

                var result = data.Data;
                $('.main-content').html(renderHtml(result.Data));
                bindEvent();
            }
        });
    }


});