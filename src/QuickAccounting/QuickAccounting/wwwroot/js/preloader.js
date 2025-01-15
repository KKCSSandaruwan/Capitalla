function hidePreloader() {
    const preloader = document.getElementById('preloader');
    const fadeDuration = 1000;

    preloader.style.transition = `opacity ${fadeDuration}ms ease-out`; 
    preloader.style.opacity = '1'; 

    setTimeout(() => {
        preloader.style.display = 'none'; 
        preloader.style.opacity = ''; 
        preloader.style.transition = ''; 
    }, fadeDuration);
}