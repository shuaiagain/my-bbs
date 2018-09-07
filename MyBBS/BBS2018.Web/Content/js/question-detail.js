$(function () {

    getQAnswerPageList();

    //写回答
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
                            debugger;
                            getQAnswerPageList();
                            layer.close(index);
                        }
                    });
                },
                success: function (layero, index) {

                }
            });
        });
    });

    //获取回答列表
    function getQAnswerPageList(pageIndex, pageSize) {

        var getQuesUrl = $('input[name="getQAnswerUrl"]').val();
        var questionId = $('input[name="questionId"]').val();

        var arguments = {
            PageIndex: pageIndex ? 1 : pageIndex,
            PageSize: pageSize ? 10 : pageSize,
            QuestionID: questionId
        };

        $.ajax({
            type: 'post',
            url: getQuesUrl,
            data: JSON.stringify(arguments),
            contentType: 'application/json;charset=utf-8',
            success: function (data) {

                if (data.Code == -200) {
                    return;
                }

                var result = data.Data;
                $('.content-list').html(renderHtml(result.Data));

                bindEvent();
            }
        });
    }

    //渲染列表html
    function renderHtml(data) {

        if (!data) return '';

        var template = '';
        for (var i = 0; i < data.length; i++) {

            template += '<div class="content-item" data-answerid="' + data[i].AnswerID + '">' +
                             '<div class="item-user clearfix">' +
                                 '<a class="user-logo floatL"><img class="logo-show img40" src="/Content/images/Login/bg-login.png" /></a>' +
                                 '<div class="user-info floatL">' +
                                     '<div class="info-name"><a class="name-show" data-uid="' + data[i].UserID + '">' + data[i].UserName + '</a></div>' +
                                     '<div class="info-domain"><a class="domain-show">your honours</a></div>' +
                                 '</div>' +
                             '</div>' +
                             '<div class="item-praise">' +
                                 '<a class="praise-show">' +
                                     '<span class="show-other">牛牛</span>' +
                                     '<span class="show-text">等</span>' +
                                     '<span class="show-count">' + data[i].PraiseCount + '</span>' +
                                     '<span class="show-last">人赞同了该回答</span>' +
                                 '</a>' +
                             '</div>' +
                             '<div class="item-answer">' +
                                 '<span class="answer-text">' + data[i].Content + '</span>' +
                             '</div>' +
                             '<div class="item-date">' +
                                 '<span class="date-text">发布于</span>' +
                                 '<span class="date-show">' + data[i].EditTimeStr + '</span>' +
                             '</div>' +
                             '<div class="content-operate clearfix"s>' +
                                 '<div class="vote floatL">' +
                                     '<div class="vote-praise  ' + (data[i].IsPraised > 0 ? "updown-active" : "") + '" data-type="1">' +
                                         '<a class="iconfont icon-arrowup"></a>' +
                                         '<a class="praise">赞同</a>' +
                                         '<a class="praise-num">' + data[i].PraiseCount + '</a>' +
                                     '</div>' +
                                     '<div class="vote-tread ' + (data[i].IsTreaded > 0 ? "updown-active" : "") + ' " data-type="2">' +
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

    //绑定事件
    function bindEvent() {

        var isVoteBack = true;
        // 赞/踩
        $('.vote-praise,.vote-tread').on('click', function () {

            if (!isVoteBack || $(this).hasClass('updown-active')) return;
            isVoteBack = false;

            var thisDom = this,
                 operate = $(this).parents('.content-item'),
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

                    $(thisDom).parents('.vote').find('.vote-praise,.vote-tread').removeClass('updown-active');
                    $(thisDom).addClass('updown-active');

                    $(thisDom).parents('.vote').find('.praise-num').text(data.Data.PraiseCount);
                }
            });

        });

        //点击评论
        $('.content-comment').on('click', function (e) {

            var thisDom = this;
            $(thisDom).parents('.content-item').siblings().each(function (i, val) {

                if ($(val).find('.sub-comment').length == 1) {
                    $(val).find('.sub-comment').remove();
                    $(val).find('.comment-num').show();
                    $(val).find('.comment-text').text('条评论');
                }
            });

            if ($(thisDom).parents('.content-item').find('.sub-comment').length == 1) {

                $(thisDom).parents('.content-item').find('.sub-comment').remove();
                $(thisDom).find('.comment-num').show();
                $(thisDom).find('.comment-text').text('条评论');
                return;
            }

            var commentListUrl = $('input[name="commentListUrl"]').val() + '?answerId=' + $(thisDom).parents('.content-item').data('answerid');
            $.get(commentListUrl, function (data) {

                $(thisDom).find('.comment-num').hide();
                $(thisDom).find('.comment-text').text('收起评论');

                //$(data).insertAfter($(thisDom).parents('.content-item'));
                $(data).appendTo($(thisDom).parents('.content-item'));
            });
        });
    }
});