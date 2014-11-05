/********************************
baidu map.js  zhaors 2013-12-25
*********************************/
var map;


/***********************************
  初始化地图
  map_canvas  显示地图的div
  longitude   中心点经度
  latitude    中心点纬度
************************************/
function initMap(map_canvas,longitude,latitude) {
    if (typeof (map) == 'undefined' || typeof (map) == '' || typeof (map) == null) {
        var opts = {type: BMAP_NAVIGATION_CONTROL_LARGE };  //初始化地图控件
        var point = new BMap.Point(longitude,latitude);        //初始化地图中心点
        map = new BMap.Map(map_canvas);
        map.addControl(new BMap.NavigationControl(opts));
        map.centerAndZoom(point, 15);                      //设置中心点坐标和地图级别
        map.enableScrollWheelZoom();                       //启用滚轮放大缩小
    }
}

/***********************************
根据经度纬度标记点
************************************/
function addMarker(point, termno) {
    var marker = new BMap.Marker(point);
    var label = new BMap.Label(termno, { offset: new BMap.Size(4, 3) });
    label.setStyle({
        color: "white",
        background: "red",
        fontSize: "12px",
        height: "10px",
        lineHeight: "10px",
        fontFamily: "微软雅黑"
    });
    marker.setLabel(label);

    //添加标记拖拽监听
    var gc = new BMap.Geocoder();
    marker.addEventListener("dragend", function (e) {
        //获取地址信息
        gc.getLocation(e.point, function (rs) {
            showLocationInfo(e.point, rs, marker, termno);
        });
    });
    //添加标记点击监听
    marker.addEventListener("click", function (e) {
        gc.getLocation(e.point, function (rs) {
            showLocationInfo(e.point, rs, marker, termno);
        });
    });
    map.addOverlay(marker);
}

/***********************************
显示地址信息窗口
************************************/
function showLocationInfo(pt, rs, mk, termno) {
    var opts = {
        width: 250,             //信息窗口宽度
        height: 120,            //信息窗口高度
        title: ""               //信息窗口标题
    }
    var addComp = rs.addressComponents;
    var addr = "<br />" + termno + "号终端位置：" + addComp.province + addComp.city + 
               addComp.district + addComp.street  + addComp.streetNumber + "<br />";
    addr += "纬度：" + pt.lat + "<br/>" + "经度：" + pt.lng;

    var infoWindow = new BMap.InfoWindow(addr, opts); //创建信息窗口对象
    mk.openInfoWindow(infoWindow);
}

/***********************************
显示终端追踪信息
************************************/
function addTrackMarker(point, termno, time) {
    var marker = new BMap.Marker(point);
    var label = new BMap.Label(termno, { offset: new BMap.Size(4, 3) });
    label.setStyle({
        color: "white",
        background: "red",
        fontSize: "12px",
        height: "10px",
        lineHeight: "10px",
        fontFamily: "微软雅黑"
    });
    marker.setLabel(label);

    //添加标记拖拽监听
    var gc = new BMap.Geocoder();
    marker.addEventListener("dragend", function (e) {
        //获取地址信息
        gc.getLocation(e.point, function (rs) {
            showTrackerInfo(e.point, rs, marker, termno, time);
        });
    });
    //添加标记点击监听
    marker.addEventListener("click", function (e) {
        gc.getLocation(e.point, function (rs) {
            showTrackerInfo(e.point, rs, marker, termno, time);
        });
    });
    map.addOverlay(marker);
}

/***********************************
显示地址信息窗口
************************************/
function showTrackerInfo(pt, rs, mk, carno,time) {
    var opts = {
        width: 100,             //信息窗口宽度
        height: 80,            //信息窗口高度
        title: ""               //信息窗口标题
    }
    var addComp = rs.addressComponents;
    var addr = "<font size=2 color=red><br />终端位置" + termno + ":" + addComp.province + addComp.city +
               addComp.district + addComp.street + addComp.streetNumber + "<br />";
    addr += "停泊时间：" + time + "<br/></font>";

    var infoWindow = new BMap.InfoWindow(addr, opts); //创建信息窗口对象
    mk.openInfoWindow(infoWindow);
}