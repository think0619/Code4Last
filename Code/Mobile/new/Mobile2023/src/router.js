import { createRouter, createWebHashHistory } from 'vue-router';
const routes = [{
        name: 'notFound',
        path: '/:path(.*)+',
        redirect: {
            name: 'main',
        },
    },
    {
        name: 'main',
        path: '/main',
        component: () =>
            import ('./view/main'),
        meta: {
            title: 'ceshi',
        },
    },
    {
        name: 'user',
        path: '/user',
        component: () =>
            import ('./view/user'),
        meta: {
            title: '会员中心',
        },
        children: [{
                path: '', //首页是默认子路由，所谓为空
                name: '/home',
                component: () =>
                    import ('@/view/goods')
            },
            {
                path: '/question',
                name: 'cart',
                component: () =>
                    import ('@/view/cart')
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
    history: createWebHashHistory(),
});

router.beforeEach((to, from, next) => {
    const title = to.meta && to.meta.title;
    if (title) {
        document.title = title;
    }
    next();
});

export { router };