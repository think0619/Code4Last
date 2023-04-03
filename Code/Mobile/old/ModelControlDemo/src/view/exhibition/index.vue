<template>

  <div class="viewcontain">
 xx
    <div style="min-height:60px">&nbsp;</div>
    <div class="bottomInfo">
      <!-- <van-image width="24" height="24" src="/static/img/yhdlogo.png" /> -->
      <div><span> </span></div>
    </div>

  </div>
</template>  

<!-- 引入样式文件 -->
<link rel="stylesheet" href="/static/css/vantindex.css"/>
<script>
import Vue from 'vue';
import { Button, Step, Steps, Image as VanImage, Skeleton, Toast, Dialog, Empty, Grid, GridItem } from 'vant';
Vue.use(Button);
Vue.use(Step);
Vue.use(Steps);
Vue.use(VanImage);
Vue.use(Skeleton);
Vue.use(Toast);
Vue.use(Dialog);
Vue.use(Empty);
Vue.use(Grid);
Vue.use(GridItem);

import { queryCert } from "@/api/index";
import { sendCMDMsg } from "@/api/index";

export default {
  components: {
    // [Field.name]: Field,
  },

  data() {
    return {
      active: 0,
      NOResult: true,
      setting: [
        { index: 'swopen_01', keyName: 'ParkModel', content: 'open01&close01' },
        { index: 'swclose_01', keyName: 'ParkModel', content: 'close01' },
        { index: 'swopen_02', keyName: 'ParkModel', content: 'open02' },
        { index: 'swclose_02', keyName: 'ParkModel', content: 'close02' },
        { index: 'swopen_03', keyName: 'ParkModel', content: 'open03' },
        { index: 'swclose_03', keyName: 'ParkModel', content: 'close03' },
        { index: 'swopen_04', keyName: 'ParkModel', content: 'open04' },
        { index: 'swclose_04', keyName: 'ParkModel', content: 'close04' },
        { index: 'swopen_05', keyName: 'ParkModel', content: 'open05' },
        { index: 'swclose_05', keyName: 'ParkModel', content: 'close05' },
        { index: 'swopen_06', keyName: 'ParkModel', content: 'open06' },
        { index: 'swclose_06', keyName: 'ParkModel', content: 'close06' },
        { index: 'swopen_07', keyName: 'ParkModel', content: 'open07' },
        { index: 'swclose_07', keyName: 'ParkModel', content: 'close07' },
        { index: 'swopen_08', keyName: 'ParkModel', content: 'open08' },
        { index: 'swclose_08', keyName: 'ParkModel', content: 'close08' },
        { index: 'swopen_09', keyName: 'ParkModel', content: 'open09' },
        { index: 'swclose_09', keyName: 'ParkModel', content: 'close09' },
      ]
    };
  },
  computed: {

  },

  methods: {
    onClickButtonSubmit(value) {
      Toast({
        message: value,
        position: 'middle',
        duration: 500
      });
    },
    clickSwitchBtn(flag, indexName) {
      var that = this;
      var handleObj = null;
      that.setting.forEach(function (value, index, array) {
        if (indexName == value.index) {
          handleObj = value;
        }
      });
      if (handleObj != null && handleObj.content!=null) {
        //cmd$ParkModel$OpenVideo3
        let msgArray=handleObj.content.split('&');
        let msgContent=[];
        msgArray.forEach(function (value, index, array) {
          let sendMsg = "cmd$" + handleObj.keyName + "$" + value;
           msgContent.push(sendMsg)
        });  
        
        //let sendMsg1 = "cmd$" + handleObj.keyName + "$" + handleObj.content+"Test";
        if (msgContent.length > 0) {
          that.doCMDSend(msgContent, "zhkjdev")
        }
      }
    },
    doCMDSend(_Msg, _Sender) {
      let that = this;
      sendCMDMsg({
        Msg: _Msg,
        Sender: _Sender,
      })
        .then((res) => {
          Toast.clear();
          console.log(res);
          if (res != null && res.data != null) {
            console.log(res.data);
            var qresult = res.data;
            if (qresult != null) {
              //success
              if (qresult.Status == 1) {
                Toast({
                  message: '发送成功',
                  position: 'middle',
                  duration: 500
                });
              } else {
                Toast({
                  message: '发送失败',
                  position: 'middle',
                  duration: 500
                });
              }
            } else {
              that.errorToast();
            }
          } else {
            that.errorToast();
          }
        })
        .catch(function (err) {
          Toast.clear();
          console.log(err);
          that.errorToast();
        });
    },
    doCertQuery() {
      // Toast.loading({
      //   duration: 0, // 持续展示 toast
      //   forbidClick: true,
      //   message: '查询中···',
      // });
      this.NOResult = true;
      this.QueryItems = [];
      this.QueryResultPicPath = "";
      let that = this;
      queryCert({
        TestNO: that.TestNO,
        QueryNO: that.QueryNO,
        QueryType: that.ActiveTabName
      })
        .then((res) => {
          Toast.clear();
          console.log(res);
          if (res != null && res.data != null) {
            console.log(res.data);
            var qresult = res.data;
            if (qresult != null) {
              //success
              if (qresult.Status == 1) {
                that.QueryItems = qresult.ResultItems;
                that.NOResult = false;
                if (qresult.PicPath == null || qresult.PicPath.length == 0) {
                  this.QueryResultPicShow = false;
                  this.QueryResultPicPath = "";
                }
                else {
                  this.QueryResultPicShow = true;
                  this.QueryResultPicPath = "http://47.106.241.45:8080/HNJYTC/" + qresult.PicPath;
                }
              } else if (qresult.Status == -1) { //testno or queryno wrong
                //查询失败 数据有误
                this.QueryResultPicShow = false;
                this.QueryResultPicPath = "";

                let alertMsg = "";
                switch (that.ActiveTabName) {
                  case "certquery": alertMsg = "您输入的证书号或查询码有误。"; break;
                  case "goldquery": alertMsg = "您输入的证签号有误。"; break;
                  case "reportquery": alertMsg = "您输入的报告号有误。"; break;
                }

                Dialog.alert({
                  title: '查询失败',
                  message: '1.' + alertMsg + '<br />2.若核实无误，请您耐心等待，数据、图片正在更新中，一般更新时间为15-20天。由于本系统的查询量较大，有部分查询可能会出现没有相关资料的信息，此时烦请您在线留言工作人员帮你查询,本中心对证书查询系统具有最终解释权。',
                  theme: 'round-button',
                }).then(() => {
                  // on close
                });

              } else {
                that.errorToast();
              }
            } else {
              that.errorToast();
            }
          } else {
            that.errorToast();
          }
        })
        .catch(function (err) {
          Toast.clear();
          console.log(err);
          that.errorToast();
        });
    },


    errorToast() {
      Toast({
        message: '发送异常',
        position: 'middle',
      });
    },
  }
};



</script>

<style   scoped>
.taaa {
  color: brown;
  font-size: 128px;
}

.vstepcontain {
  background: #efefef;
  margin: 16px;
  box-shadow: 0px 0px 10px #D0D8E5;
  border-radius: 1ex;
  padding-top: 15px;
}

::v-deep .van-grid-item__content--center {
  padding: 8px !important;
}

.viewcontain {
  height: calc(100% - 60px);
}

.bottomInfo {
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #ececec;
  position: fixed;
  bottom: 30px;
  width: 100%;
}

.bottomInfo>div {
  margin-left: 8px;
  font-size: 0.9em;
}
</style>
 