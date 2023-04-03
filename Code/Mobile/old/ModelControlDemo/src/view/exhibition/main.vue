<template>
  <div class="viewcontain"> 
    <div style="margin-bottom:50px">
      <van-button type="primary" @click="clickSwitchBtn22()">打开 08</van-button>
    </div>
  </div>
</template>  

<!-- 引入样式文件 -->
<link rel="stylesheet" href="/static/css/vantindex.css"/>
<script>
import Vue from 'vue';
import { Button,Toast, Tabbar,TabbarItem, } from 'vant';
Vue.use(Button); 
Vue.use(Toast); 
Vue.use(Tabbar);
Vue.use(TabbarItem); 
 
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
  setup() {
  
  },
  methods: {
    clickSwitchBtn22(){ 
     location.href="http://10.10.20.243:8080/"
    },
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
 