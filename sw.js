const CACHE='valleytapping-v17';const FILES=['./','./index.html','./manifest.webmanifest','./manifest.webmanifest?v=4','./iconmimi.png','./iconmimi.png?v=4','./assets/sprites/environment/farm-scene.svg?v=1','./assets/sprites/pets/lillia.svg?v=1','./mishi.svg','./assets/sprites/pets/mishi.svg?v=1','./assets/sprites/crops/carrot.svg?v=2','./assets/sprites/crops/stages/carrot.svg?v=1','./assets/sprites/crops/stages/wheat.svg?v=1','./assets/sprites/crops/stages/strawberry.svg?v=1','./assets/sprites/crops/stages/potato.svg?v=1','./assets/sprites/crops/stages/tomato.svg?v=2','./assets/sprites/crops/stages/blueberry.svg?v=2','./assets/sprites/crops/carrot.svg?v=1','./assets/sprites/crops/wheat.svg?v=1','./assets/sprites/crops/strawberry.svg?v=1','./assets/sprites/crops/potato.svg?v=1','./assets/sprites/crops/tomato.svg?v=1'];self.addEventListener('install',e=>{self.skipWaiting();e.waitUntil(caches.open(CACHE).then(c=>c.addAll(FILES)))});self.addEventListener('activate',e=>e.waitUntil(caches.keys().then(keys=>Promise.all(keys.filter(k=>k!==CACHE).map(k=>caches.delete(k)))).then(()=>self.clients.claim())));self.addEventListener('fetch',e=>{
 const url=new URL(e.request.url);
 if(e.request.mode==='navigate'||url.pathname.endsWith('/release-notes.json')){
  e.respondWith(fetch(e.request,{cache:'no-store'}).then(response=>{
   if(e.request.mode==='navigate'&&response.ok){const copy=response.clone();caches.open(CACHE).then(cache=>cache.put('./index.html',copy));}
   return response;
  }).catch(()=>caches.match(e.request).then(cached=>cached||caches.match('./index.html'))));
  return;
 }
 e.respondWith(caches.match(e.request).then(r=>r||fetch(e.request)));
});