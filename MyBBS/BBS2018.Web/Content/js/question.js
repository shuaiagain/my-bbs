$(function () {

    getQuestionList();

    //查询
    $('.btn-search').on('click', function (e) {

        getQuestionList($.trim($('.search-key ').val()), 1);
    });

    //查询
    $('.search-key ').on('keydown', function (e) {

        if (e.keyCode == 13) {
            $('.btn-search').trigger('click');
        }
    });

    //提问
    $('.ask ').on('click', function (e) {

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

                            $('.btn-search').trigger('click');
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

    //获取列表
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

    //绑定事件
    function bindEvent() {

        var isVoteBack = true;
        // 赞/踩
        $('.vote-praise,.vote-tread').on('click', function () {


            if (!isVoteBack || $(this).hasClass('updown-active')) return;
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
                    'PraiseOrTread': $(this).data('type')
                }),
                contentType: 'application/json;charset=utf-8',
                success: function (data) {
                    isVoteBack = true;

                    if (data.Code < 0) {
                        return false;
                    }

                    $(thisDom).parents('.vote ').find('.vote-praise,.vote-tread').removeClass('updown-active');
                    $(thisDom).addClass('updown-active');

                    $(thisDom).parents('.vote').find('.praise-num').text(data.Data.PraiseCount);
                }
            });

        });
    }

    //渲染列表html
    function renderHtml(data) {

        if (!data) {

            return '<div class="qu-nodata">' +
                        '<span>你的问题比较特别囧，赶紧去提问吧:)</span>' +
                   '</div>';
        }

        var template = '';
        var quDetialUrl = $('input[name="questionDetail"]').val() + '?questionId=';
        for (var i = 0; i < data.length; i++) {

            template += '<div class="content-item">' +
                          '<div class="item-user">' +
                              '<a class="user-headlogo"><img class="headlogo-item img24" src="/Content/images/Login/bg-login.png" /></a>' +
                              '<a class="user-name">' + data[i].UserName + '</a>' +
                              '<div class="user-intro ellipsis" title="用户介绍">用户介绍</div>' +
                          '</div>' +
                          '<div class="content-ask">' +
                              '<a class="ask-question" target="_blank" href="' + quDetialUrl + data[i].QuestionID + '">' + data[i].Title + '</a>' +
                          '</div>';

            if (data[i].Content) {
                template += '<div class="content-answer">' +
                                '<div class="answer-wrap clearfix">' +
                                    '<div class="answer-pic floatL" style="display:none;">回答图片</div>' +
                                    '<div class="answer-words floatL">' +
                                       '<span class="words-summary ellipsis-mu">' + data[i].Content + '</span>' +
                                       '<p class="words-all"></p>' +
                                       '<a class="words-click">阅读全文</a>' +
                                    '</div>' +
                                '</div>' +
                              '</div>';
            } else {

                //template += '<div class="content-answer">' +
                //            '<div class="answer-wrap clearfix">' +
                //                '<div class="wrap-con">' +
                //                    '<a class="iconfont icon-answer"></a>' +
                //                    '<a class="con-qu">回答</a>' +
                //                '</div>' +
                //            '</div>' +
                //          '</div>';
            }

            if (data[i].Content) {
                template += '<div class="content-operate clearfix" data-answerid="' + data[i].AnswerID + '">' +
                                '<div class="vote floatL">' +
                                    '<div class="vote-praise ' + (data[i].IsPraised > 0 ? "updown-active" : "") + '" data-type="1">' +
                                       '<a class="iconfont icon-arrowup"></a>' +
                                       '<a class="praise">赞同</a>' +
                                       '<a class="praise-num">' + data[i].TotalPraise + '</a>' +
                                    '</div>' +
                                    '<div class="vote-tread ' + (data[i].IsTreaded > 0 ? "updown-active" : "") + '" data-type="2">' +
                                        '<a class="iconfont icon-arrowdown"></a>' +
                                    '</div>' +
                                '</div>' +
                                '<div class="content-comment floatL">' +
                                    '<div class="comment-wrap">' +
                                       //'<a class="iconfont icon-comment"></a>' +
                                       '<a class="comment-num">0</a>' +
                                       '<span class="comment-text">条评论</span>' +
                                    '</div>' +
                                '</div>' +
                             '</div>' +
                        '</div>';
            } else {

                template += '<div class="content-operate clearfix no-answer">不要着急，静等解惑</div>' +
                        '</div>';
            }
        }
        return template;
    }

});