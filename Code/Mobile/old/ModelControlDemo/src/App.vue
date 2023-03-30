<template>
  <div id="app">
    <router-view />

  </div>

</template>
<!-- 引入样式文件 -->
 
 
<style>
body {
  height: 100%;
  font-size: 15px;
  background-color: #f8f8f8;
  -webkit-font-smoothing: antialiased;
}

html,
body,
#app {
  height: 100%;
  margin: 0;
  padding: 0; 
}

html,body{
    overflow-x: hidden;
  position: relative;
  width: 100%;
  height: 100%;
}
</style>
