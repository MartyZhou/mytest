var gmap;

var clicktimes = 0;
var photos = [
    { name: 'Sanya', url: 'https://0nnwaa.bn1303.livefilestore.com/y3mcfj_pf4G6erWYpiOaz-vAwe489-H9WxGEfOM1vAeZucJTLP4w89S05-PAN7oJd5dbOVOgctKYmnXeuyE0sorMqV72YVCPmdZu6rFK4Jd2KgHBhdCMkA82U9LwGD2v_-XH9W5_-ruprD-2PieTTNlpBML3-cmoIQ9D4Hh3NPCh58?width=495&amp;height=660&amp;cropmode=none' },
    { name: 'Shanhaiguan', url: 'https://luotda.bn1303.livefilestore.com/y3mmsPBOycAaFEMZ4GnhzOC-SR2JXuvW9WKC03YNAthBXkGhMtF8qI6pMR5cDFiQOqt6djoUE6xONc2tWSfdczpriwgfVOIdciRFVLc1BbEhJmaPR_XLW2WcWj-eAAX8SrsoXOUEoXfdCFr8kKB923oJPbD-BfiMaom-ygK5A2Ev7o?width=660&amp;height=495&amp;cropmode=none' },
    { name: 'Caijiazhuang, Shanxi', url: 'https://kenpag.bn1303.livefilestore.com/y3mDueQRoqt02wL-GVkWHk2RRnzA1oH9n4fShQUZvrmCCCeLanjhKtiAdIzxDdb4nYWVDL8jka8eJ8GJ9ZRxVVgI-LSjIPJfqK7yWEiOLqpp-88R0jyPHx1gyGDdskFpLa-wukH9AHObsoFx-LEP_iVD5XbEZUzrartbbhOKcLaT5g?width=660&amp;height=495&amp;cropmode=none' },
    { name: 'Datong, Shanxi', url: 'https://lkpcrg.bn1303.livefilestore.com/y3mrscRc4oCDG0wXwfPKQ-lvkBaFeWDabOnM-xm89LD--p22qaojP9KGQRMnUNEeyB5c1vWZcr_kcPXROu6_FIYgjPzGBHVyWZ81LW3dTl6rkvNdLySK7kisn61zLPP1gvOq5ZLJiOqUci6Oq460MIPwzFLZDBKM1b_jLDPYwxAID8?width=660&amp;height=264&amp;cropmode=none' },
    { name: 'Tuling, Shanxi', url: 'https://lemz3w.bn1303.livefilestore.com/y3m8nrO1xD6oM0xIBjodR531TG_EybhXnb136ZVJmZ4C_o5bAL3g2_WVy17J2T4dRjf-zVSzE9Ius2Q90iQxy8oi_MUyqKgiNo7S-2yVXFdgNL6V-M2VP_aBH3Gv1k7tSeAkYdxcrDlnUFriC6ilv8YMlF4zj54nwJdoW6VEr9asz4?width=660&amp;height=149&amp;cropmode=none' },
    { name: 'Hukou, Shanxi', url: 'https://iunpag.bn1303.livefilestore.com/y3mazGTPItjub2jamgMmHV_rJzIDPV0PCueBCtX5b25UcQq3j-W69hrYPE43uQJeXumiQ13_XZ93VULSGmKplbFVbYjcXpiZ0gdWE1ZFUl29hwrRYjyoecMyRv3PrUADJW-ut0nIBUukjO3rsTFILM-dDIvb3KcVvuj53oAJIoEOD8?width=660&amp;height=221&amp;cropmode=none' },
    { name: 'Hanging Temple, Shanxi', url: 'https://lunigq.bn1303.livefilestore.com/y3mdoD3YuVFGvx6ue3WTnFay7AN5pQgYU8nPXWKwHUMGrxYljgGpjKkEnz8J24SvtkoZVnCVh3hD65z7vOBaXiU7w0qgAhHX50ztbA-Pr_W7pG6NKFdhZMjHZPAlfE2LjiASd4RZk9H1lxCtJFCpFWEd19FVbfNkEjAPSP7Zmto1k0?width=660&amp;height=205&amp;cropmode=none' },
    { name: 'Wulanbutong', url: 'https://kkn3iw.bn1303.livefilestore.com/y3m7J2Iar2490CAGqhBdimVmmHRZB6MZxNILm7R7QhGohJ2tRlT_B3gSjoRFDWfSn5zZx6kwcDM5mhGh2uqeEZCEDIDwjgB5KzLLHWO_RYSuDoqBbEe5wdDiZHqP2xs-TF2txDcuAKV7W2cUKJpsmVa4lfINPnshAJcLPM14lH2pds?width=660&amp;height=495&amp;cropmode=none' },
    { name: 'Malaysia', url: 'https://ienigq.bn1303.livefilestore.com/y3m3wRha_wl4qKYDhODJUd9EzJdj80rhgX0SrWscER5XV47NsHMNHVsjT6xgyKMFzH7IN_AoKAdhVouOLn8cC6Ip8imaCTTZEA32z6nXaOOgFhVp_kkQOVh7-vrSfrWSaRqAt4qFGn3dKaktRBBsoHEEMZ8rdbvPY4jsbOmF9aZAg8?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Bashang, Chengde', url: 'https://iunigq.bn1303.livefilestore.com/y3mdS-mfQ0AtzysIVoOswdpqvJIcbudUEBYXVkkAik81a-utqijrfz8aux6-kZXo70r_TDcccCnLoK8o44_pselxWbwGFSA379tO_jgB_aIs_fbjY44K6UDVU8iPyI1uW2v5G0V11BZSjKEbUodvoqpzIsTO8FCBE4PWR1tI3OLIZQ?width=660&amp;height=495&amp;cropmode=none'},
    { name: 'Provincetown, Boston', url: 'https://kepvzw.bn1303.livefilestore.com/y3mw0ag478Xp78C9KQu5uE0iKZzmikE8a_V-gLBPnb9tHykL9UPYOK8VVoNKlXvnOOPXEyjp-tawD68fW9DjVL5VMO8sXsMf-kRRRYSUoE68pr8OgkZBMiZkF2Le869uKbLwx7ciqgiJjLuaUO5dDh0TSnxRoTCJgud3ag0KGmXFjQ?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Boston', url: 'https://kupvzw.bn1303.livefilestore.com/y3mtIUhhpayZjKzbsAioj0AzLDA4MHkd3cUsSDGDTs9XmT_WGJ6shIpUAYZUiRJ4Dx9Gx2TthnYfVNtuZzDRuUT_SI5KUPTCPitDvxO1Bx23RSR--ske49By0SOkauzI-b8I1NV4DISKkBcX443fhs-jN8lrL7vJzIptmm4nlxk2rs?width=660&amp;height=495&amp;cropmode=none'},
    { name: 'Yueyang', url: 'https://k0pvzw.bn1303.livefilestore.com/y3mDXcU7RX95jx2aNbPhINjs3-Pj89Pbkbs6KRJchz70j6HyhJrLLVeJtCJwx3m59md_lhjh6fxPbAKQkXmBYvGOZ8OzSFHTIkNlmMNMXOzFGhKFsMT3_5VnS2WuKSooKUuAmYVKXa_1CSPBU-3zdPeLkJbCYOjSa9jLz6Od4g2h1A?width=660&amp;height=660&amp;cropmode=none'},
    { name: 'Cluj, Romania', url: 'https://lepvzw.bn1303.livefilestore.com/y3m-VE81acc1B5qpNJEQtivFaxTTuN6bSy_gc3y-H13PXdSvVNQr0w6ArQt-zzH5IkXFI3xR7FkTJZ6fF4ZvpENqd-w_UXM8DU824FLMTgNRPclI80KExHpKr5zuy2pfI7QV-iBQ00x21rsbz34ydDEDk9y3W-u6jXGZ4jp4Ulo3JI?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Xihu, Changde, Hunan', url: 'https://lkpvzw.bn1303.livefilestore.com/y3mdeIFLSyJwxoapHofw-hJvC2hCF_J8oyeLfItKu-ox2_DQUhvTA_hXUkhsYuACq7jxf2bTUnueqyY0iPgTD346UjM37QYd7sjxYrDDY5LxGU_2Wq6pKBz6TCmAnTaajG42mBnwiGvyWPVkPC10QUCiMafX64X6ImlV_eQTS8fW6I?width=660&amp;height=495&amp;cropmode=none'},
    { name: 'Zhangjiajie, Hunan', url: 'https://iepvzw.bn1303.livefilestore.com/y3mT__4SJYR5KhQky6_PKNwdzIuVJWZDgBdircgEtQouMfxAKX0umukP2RiiHg92EdxsmJyzNGeTAoMuuNWSsG7eoInvFCRP1zvcVGZuJM0H_rqT6uD92AJ6gje0IuzCB1aEvUIFrGOpJ44r3ykRpVKEnB-JgCvLfgjo5oxdOwWE_A?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Fenghuang, Hunan', url: 'https://kem68w.bn1303.livefilestore.com/y3mkoLfJ-N8T8pReQCen_YeLBdJ07PY0wQE_Nw0Bm6L5BChZrMkCfqqIvPXRKu7eCpO5_2pvOqdS9HqoJng66lbp8CjE8GF-AGD7ZJCjt8LcPBSQWHV1Lr5_LNRgkO6Nn_gKojqfda3eKgfvgS62iNlcDmRsI8MJVyo1u8Q8IMKSnk?width=660&amp;height=495&amp;cropmode=none'},
    { name: 'Changsha, Hunan', url: 'https://kum68w.bn1303.livefilestore.com/y3mJiTUVnemJ0HwIfhBcFNleulBCo946WlF_BIBMloRBMHzhf0MjK6QX9e5zN-9jqipCV2wBrvpSBMzdVf4jQp6mIGkiFvBxQM55cW7RkYuIxnBfe7fH_Sppe5_HAYDy3_aUSzJeGL_kOe_Z_gc1YNnHc8mQVpLFxDRDpOdP6cxAz0?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Elephent Sanctuary, Chiangmai', url: 'https://0xni2g.bn1303.livefilestore.com/y3mBLOw5O0YDExjXPVobxwhTJW4U15-q4-qJyddTl4bsrJiA7qNXY25wQRDFKSCJTT9EA_yXMJwwg9niKMDFIlKC57tlNqX1S03c0zlH_JucaLDbaoPlaNgPMhC8EcUEf2D2MAi3cpg2w_PnRjYYV0KJi81n27ySunTyhBRExZvJ_Q?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Zipline, Chiangmai', url: 'https://xno78w.bn1303.livefilestore.com/y3mjBawgf6P0u8nHE1KGNRgtWmaflNGu_rdOJIPe2hN6RyIETEew0GgguUaefCPw9fML8OXGdmfvlb_kJpfjJ1H_-Z3OOQAdpIV0b8XjFY4dBDQ8bc_gHVDH9ChVM0zF5kAYx8ISOWRMVPb2E1zZtRaHB6L9E6STVgbHaA-yVASvww?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Angkor Wat, Siem Reap', url: 'https://0nni2g.bn1303.livefilestore.com/y3m0AxVarTjpYO7opc8dB1xH3UWnyqYN6gbn9iy-5M_ZzlJQ-ttm1vj2nHv-nCltMpuBHlIhHGSEQOAZS9ME_4lbP2xcOebNotCau-i9d29Mc2yqQYXxKNqLLzhsxb2gPW9HAt5W4CBorkRiDMMv1PR9nvgAtDFrWy0q-mUTyFETdU?width=660&amp;height=495&amp;cropmode=none'},
    { name: 'Battambang, Combodia', url: 'https://k0m68w.bn1303.livefilestore.com/y3m5o3duGCzJntXilB_O2CeRxciE5o_sDrA80awier0QUnqeuVEPfmioqJiiLyN6fSl4Wog2VozObAwUNNhfzkr3P7SA51V-Y622F2tCBmlN5-_YYePUjKWpnf9tR6g6RHx5hBJr75e-DGN4t9kcD8aCdzdo7UDytsDISzZuvlCGBM?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Nandaihe, Hebei', url: 'https://lem68w.bn1303.livefilestore.com/y3m8fFwUb9fZUKY1J3dzcBGHrUhuPA18awt3YqDMsaYgEwWgpSwTo0PmZRuqVt-jFHd-aDR_6jWrDsuSrZgaFsT3Vl0iy51PF7aYqY6aY5s9Q_fpy9uqoTugBegNDyUc4w5ZbbgxyvfwFcpZXAmReamL-QSWie8xYkZU2nMii1W8CQ?width=495&amp;height=660&amp;cropmode=none'},
    { name: 'Qingdao', url: 'https://lum68w.bn1303.livefilestore.com/y3miCDJZsH3k4XetANqVEb6GguiWXCreh_faIF2-rsdqBOnUBCdWawx-hSzFZy5W1oNxjFlMaPyQiAYMiuwsgSgVOKaiPop8AAk8xTc4PZ4fsrXUUxuC3-AHws1vzTCN_W4FRftZLsydnCNlCeh53FD968wwa7RL6bVpW3482ZcoF0?width=660&amp;height=495&amp;cropmode=none'},


];

function initMap() {
    var uluru = { lat: 45.363, lng: 131.044 };
    var map = new google.maps.Map(document.getElementById('map'), {
        zoom: 3,
        center: uluru
    });
    gmap = map;
}

/*document.getElementById("imgtest").onclick = function () {
    EXIF.getData(this, function () {
        var exifdata = this.exifdata;
        var lat = exifdata.GPSLatitude;
        var lon = exifdata.GPSLongitude;
        var latRef = exifdata.GPSLatitudeRef;
        var lonRef = exifdata.GPSLongitudeRef;
        var latResult = (lat[0] + lat[1] / 60 + lat[2] / 3600) * (latRef == "N" ? 1 : -1);
        var lonResult = (lon[0] + lon[1] / 60 + lon[2] / 3600) * (lonRef == "W" ? -1 : 1);

        var location = { lat: latResult, lng: lonResult };

        if (gmap) {
            var testMarker = new google.maps.Marker({
                position: location,
                map: gmap
            })
        }
    });
}*/

document.getElementById("objecturltest").onclick = function () {
    if (clicktimes < photos.length) {
        var http = new XMLHttpRequest();
        http.open("GET", photos[clicktimes].url, true);
        http.responseType = "blob";
        http.onload = function (e) {
            if (this.status === 200) {
                var image = new Image();
                image.onload = function () {
                    EXIF.getData(image, function () {
                        var exifdata = this.exifdata;
                        var lat = exifdata.GPSLatitude;
                        var lon = exifdata.GPSLongitude;
                        var latRef = exifdata.GPSLatitudeRef;
                        var lonRef = exifdata.GPSLongitudeRef;
                        if (lat && lon && latRef && lonRef) {
                            var latResult = (lat[0] + lat[1] / 60 + lat[2] / 3600) * (latRef == "N" ? 1 : -1);
                            var lonResult = (lon[0] + lon[1] / 60 + lon[2] / 3600) * (lonRef == "W" ? -1 : 1);

                            var location = { lat: latResult, lng: lonResult };

                            if (gmap) {
                                var testMarker = new google.maps.Marker({
                                    position: location,
                                    map: gmap
                                })
                            }
                        }
                    });
                };
                image.src = URL.createObjectURL(http.response);
            }
        };
        http.send();
    }
    clicktimes++;
}