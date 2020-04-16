<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="captcha_demo.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="captcha.ashx?action=getscript"></script>
    <script src="https://cdn.bootcss.com/jquery/3.5.0/jquery.js"></script>
</head>
<body>
    <div id="test" ></div>
    <script>
        var mangoCaptcha;
        document.getElementById("test").initCaptcha(function (m) {
            mangoCaptcha = m;
            refreshCaptcha();

        });
        document.getElementById("test").addEventListener("captcha_refresh",
            function (event) {
                refreshCaptcha();
            });

        document.getElementById("test").addEventListener("captcha_ok", function (event) {
            var key = event.detail.key;
            var selectValues = event.detail.selectValues;
            $.ajax({
                url: "captcha.ashx?action=valcaptcha", // 目标资源
                cache: false, //true 如果当前请求有缓存的话，直接使用缓存。如果该属性设置为 false，则每次都会向服务器请求
                async: false, //默认是true，即为异步方式
                data: { key, selectValues: selectValues.join(',') },
                dataType: "json", // 服务器响应的数据类型
                type: "POST", // 请求方式
                success: function (data) {
                    if (data.success) {
                        alert('校验成功');
                    } else {
                        alert('校验失败');
                        refreshCaptcha();
                    }
                }
            });
        });

        function refreshCaptcha() {
            $.get('captcha.ashx?action=getcaptcha',
                function (res) {
                    var result = JSON.parse(res);
                    mangoCaptcha.loadImage("data:image/png;base64," + result.base64Img, result.key);
                });
        }
    </script>
</body>
</html>
