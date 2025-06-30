
let header = document.querySelector("header");

window.addEventListener("scroll", () => {
    header.classList.toggle("shadow", window.scrollY > 0);
});

// Filter
$(document).ready(function () {
    // Filtreleme i�lemi
    $(".filter-item").click(function () {
        const value = $(this).attr("data-filter");  // Se�ilen kategori de�eri
        if (value == "all") {
            // E�er "all" kategorisi se�ildiyse t�m bloglar� g�ster
            $(".post-box").show(1000);
        } else {
            // Se�ilen kategoriye uyan postlar� g�ster, di�erlerini gizle
            $(".post-box").each(function () {
                var categories = $(this).data("categories"); // Kategori bilgisini al
                if (categories == value) {
                    $(this).show(1000);  // E�er kategori uyu�uyorsa g�ster
                } else {
                    $(this).hide(1000);  // E�er kategori uymuyorsa gizle
                }
            });
        }
    });

    // Aktif filtreyi i�aretleme
    $(".filter-item").click(function () {
        $(this).addClass("active-filter").siblings().removeClass("active-filter");
    });
});

// Scrollbar Styles
document.addEventListener("DOMContentLoaded", () => {
    const style = document.createElement('style');
    style.textContent = `
      ::-webkit-scrollbar {
        width: 12px;
      }

      ::-webkit-scrollbar-track {
        background: #f1f1f1;
      }

      ::-webkit-scrollbar-thumb {
        background: rgb(250, 160, 75);
        border-radius: 6px;
      }

      ::-webkit-scrollbar-thumb:hover {
        background: rgba(250, 160, 75, 0.8);
      }
    `;
    document.head.appendChild(style);
});
