import { createRouter, createWebHashHistory, createWebHistory, } from 'vue-router';



const routes = [{
        path: '/',
        redirect: '/home'
    },
    {
        path: '/home',
        name: '首页',
        component: import ('./view/home'),
        meta: {
            title: '新技术、新工艺、新装备',
        },
    },
    {
        path: '/main',
        name: '标签页面',
        component: import ('./view/main'),
        children: [{
                name: 't1',
                path: 't1',
                component: () =>
                    import ('./view/main/tab1'),

            },
            {
                name: 't2',
                path: 't2',
                component: () =>
                    import ('./view/main/tab2'),

            },
            {
                name: 't3',
                path: 't3',
                component: () =>
                    import ('./view/main/tab3'),

            },
            {
                name: 't4',
                path: 't4',
                component: () =>
                    import ('./view/main/tab4'),

            },
        ]
    },

    {
        name: 'cart',
        path: '/cart',
        component: () =>
            import ('./view/cart'),
        meta: {
            title: '购物车',
        },
    },
    {
        name: 'goods',
        path: '/goods',
        component: () =>
            import ('./view/goods'),
        meta: {
            title: '商品详情',
        },
    },
];

const router = createRouter({
    routes,
    history: createWebHashHistory(), //createWebHistory(), createWebHashHistory
});

router.beforeEach((to, from, next) => {
    const title = to.meta && to.meta.title;
    if (title) {
        document.title = title;
    }
    next();
});

export { router };