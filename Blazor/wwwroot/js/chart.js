 window.drawPieChart = (canvasId, data, labels) => {
        const canvas = document.getElementById(canvasId);
        const ctx = canvas.getContext("2d");

        const total = data.reduce((a, b) => a + b, 0);
        const colors = ["#FF6384", "#36A2EB", "#FFCE56", "#4CAF50", "#FF9800", "#9C27B0"];

        let startAngle = 0;
        const slices = [];

        for (let i = 0; i < data.length; i++) {
            const sliceAngle = 2 * Math.PI * data[i] / total;

            // ????? ?????
            slices.push({
                start: startAngle,
                end: startAngle + sliceAngle,
                value: data[i],
                label: labels[i],
                color: colors[i % colors.length]
            });

            // ??? ?????
            ctx.fillStyle = colors[i % colors.length];
            ctx.beginPath();
            ctx.moveTo(canvas.width / 2, canvas.height / 2);
            ctx.arc(
                canvas.width / 2,
                canvas.height / 2,
                Math.min(canvas.width / 2, canvas.height / 2) - 20,
                startAngle,
                startAngle + sliceAngle
            );
            ctx.closePath();
            ctx.fill();

            startAngle += sliceAngle;
        }

        // Tooltip ??? ????? ??????
        canvas.addEventListener("mousemove", (event) => {
            const rect = canvas.getBoundingClientRect();
            const x = event.clientX - rect.left - canvas.width / 2;
            const y = event.clientY - rect.top - canvas.height / 2;
            const angle = Math.atan2(y, x);
            const distance = Math.sqrt(x * x + y * y);

            const radius = Math.min(canvas.width / 2, canvas.height / 2) - 20;

            ctx.clearRect(0, 0, canvas.width, canvas.height);

            // ????? ?????
            slices.forEach(slice => {
                ctx.fillStyle = slice.color;
                ctx.beginPath();
                ctx.moveTo(canvas.width / 2, canvas.height / 2);
                ctx.arc(
                    canvas.width / 2,
                    canvas.height / 2,
                    radius,
                    slice.start,
                    slice.end
                );
                ctx.closePath();
                ctx.fill();
            });

            // ??? ?????? ???? ???????
            if (distance <= radius) {
                let hoverSlice = slices.find(s => {
                    let a = angle >= 0 ? angle : (2 * Math.PI + angle);
                    return a >= s.start && a <= s.end;
                });

                if (hoverSlice) {
                    ctx.fillStyle = "black";
                    ctx.font = "14px Arial";
                    ctx.textAlign = "center";
                    ctx.fillText(
                        `${hoverSlice.label}: ${hoverSlice.value}`,
                        canvas.width / 2,
                        canvas.height - 10
                    );
                }
            }
        });
    };
