import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import Finished from "../views/Finished.vue";
import Home from "../views/Home.vue";
import InPlay from "../views/InPlay.vue";
import NoPlay from "../views/NoPlay.vue";
import PostPlay from "../views/PostPlay.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/about",
    name: "About",
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () =>
      import(/* webpackChunkName: "about" */ "../views/About.vue"),
  },
  {
    path: "/game/finished",
    name: "Finished",
    component: Finished,
  },
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  {
    path: "/game/inplay",
    name: "InPlay",
    component: InPlay,
  },
  {
    path: "/game/noplay",
    name: "NoPlay",
    component: NoPlay,
  },
  {
    path: "/game/postplay",
    name: "PostPlay",
    component: PostPlay,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
