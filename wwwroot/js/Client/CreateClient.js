(function () {
    document.addEventListener('DOMContentLoaded', function () {
        const form = document.getElementById('clientForm');
        if (!form) return;

        async function submitForm(event) {
            event.preventDefault();

            const formData = {
                Name: form.Name.value,
                Phone: form.Phone.value,
                Email: form.Email.value
            };

            try {
                const response = await fetch('/Client/CreateClient', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(formData)
                });

                if (response.ok) {
                    $('#clientModal').modal('hide');
                    alert('Sucesso', 'Cliente cadastrado com sucesso!')
                    location.reload();
                } else {
                    const error = await response.text();
                    console.log(error)
                }
            } catch (error) {
                console.log(error)
            }
        }

        form.addEventListener('submit', submitForm);
    });
})();
