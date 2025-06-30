
let header = document.querySelector("header");

window.addEventListener("scroll", () => {
    header.classList.toggle("shadow", window.scrollY > 0);
});

// Filter
$(document).ready(function () {
    // Filtreleme iþlemi
    $(".filter-item").click(function () {
        const value = $(this).attr("data-filter");  // Seçilen kategori deðeri
        if (value == "all") {
            // Eðer "all" kategorisi seçildiyse tüm bloglarý göster
            $(".post-box").show(1000);
        } else {
            // Seçilen kategoriye uyan postlarý göster, diðerlerini gizle
            $(".post-box").each(function () {
                var categories = $(this).data("categories"); // Kategori bilgisini al
                if (categories == value) {
                    $(this).show(1000);  // Eðer kategori uyuþuyorsa göster
                } else {
                    $(this).hide(1000);  // Eðer kategori uymuyorsa gizle
                }
            });
        }
    });

    // Aktif filtreyi iþaretleme
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
