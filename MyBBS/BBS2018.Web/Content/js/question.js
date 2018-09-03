$(function () {

    getQuestionList();
    bindEvent();

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

    function getQuestionList(keyWord, pageIndex, pageSize) {

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

                if (data.Code < 0) {

                }
                var result = data.Data;
                $('.main-content').html(renderHtml(result.Data));
                bindEvent();
            }
        });


    }

    function bindEvent() {

        var isVoteBack = true;
        // 赞/踩
        $('.vote-praise,.vote-tread').on('click', function () {

            if (!isVoteBack) return;
            isVoteBack = false;

            var thisDom = this,
                 operate = $(this).parents('.content-operate'),
                 saveUrl = $('input[name="savePraiseTreadUrl"]').val();

            $.ajax({
                type: 'post',
                url: saveUrl,
                data: JSON.stringify({
                    'BindTableID': operate.data('answerid'),
                    'BindTableName': 'bbsanswer',
                    'PraiseOrTread': 1,
                    'ID': operate.data('id')
                }),
                contentType: 'application/json;charset=utf-8',
                success: function (data) {
                    isVoteBack = true;

                    if (data.Code < 0) {
                        return false;
                    }

                    $('.vote-praise,.vote-tread').removeClass('updown-active');
                    $(thisDom).addClass('updown-active');

                    $(thisDom).parents('.vote').find('.praise-num').text(data.Data.Count);
                }
            });

        });
    }

    function renderHtml(data) {

        var quDetialUrl = $('input[name="questionDetail"]').val() + '?questionId=';
        var template = '';
        for (var i = 0; i < data.length; i++) {

            template += '<div class="content-item">' +
                          '<div class="item-user">' +
                              '<a class="user-headlogo"><img class="headlogo-item img24" src="/Content/images/Login/bg-login.png" /></a>' +
                              '<a class="user-name">' + data[i].UserName + '</a>' +
                              '<div class="user-intro ellipsis" title="用户介绍">用户介绍</div>' +
                          '</div>' +
                          '<div class="content-ask">' +
                              '<a class="ask-question" target="_blank" href="' + quDetialUrl + data[i].QuestionID + '">' + data[i].Title + '</a>' +
                          '</div>' +
                          '<div class="content-answer">' +
                            '<div class="answer-wrap clearfix">' +
                                '<div class="answer-pic floatL" style="display:none;">回答图片</div>' +
                                '<div class="answer-words floatL">' +
                                   '<span class="words-summary ellipsis-mu">' + data[i].Content + '</span>' +
                                   '<p class="words-all"></p>' +
                                   '<a class="words-click">阅读全文</a>' +
                                '</div>' +
                            '</div>' +
                          '</div>' +
                          '<div class="content-operate clearfix" data-answerid="4" data-id="">' +
                            '<div class="vote floatL">' +
                                '<div class="vote-praise">' +
                                   '<a class="iconfont icon-arrowup"></a>' +
                                   '<a class="praise">赞同</a>' +
                                   '<a class="praise-num">' + data[i].TotalPraise + '</a>' +
                                '</div>' +
                                '<div class="vote-tread">' +
                                    '<a class="iconfont icon-arrowdown"></a>' +
                                '</div>' +
                            '</div>' +
                            '<div class="content-comment floatL">' +
                                '<div class="comment-wrap">' +
                                   '<a class="iconfont icon-comment"></a>' +
                                   '<a class="comment-num">0</a>' +
                                   '<span class="comment-text">条评论</span>' +
                                '</div>' +
                            '</div>' +
                          '</div>' +
                      '</div>';
        }
        return template;
    }

});