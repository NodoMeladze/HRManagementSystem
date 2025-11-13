/* ===================================
   HR Management System - Custom JavaScript
   =================================== */

// Wait for DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function () {
    console.log('HR Management System - JavaScript Loaded');

    // Initialize all features
    initFormSubmitLoading();
    initAutoHideAlerts();
    initReleaseDateToggle();
    initFormValidation();
    initConfirmDialogs();
    initTooltips();
});

/* ===================================
   Form Submit Loading State
   =================================== */
function initFormSubmitLoading() {
    const forms = document.querySelectorAll('form[method="post"]');

    forms.forEach(form => {
        form.addEventListener('submit', function (e) {
            const submitButton = form.querySelector('button[type="submit"]');

            if (submitButton && !submitButton.disabled) {
                // Store original text
                const originalText = submitButton.innerHTML;
                submitButton.setAttribute('data-original-text', originalText);

                // Disable and show loading
                submitButton.disabled = true;
                submitButton.innerHTML = '<span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>იტვირთება...';

                // Re-enable after 15 seconds as fallback (in case of error)
                setTimeout(() => {
                    if (submitButton.disabled) {
                        submitButton.disabled = false;
                        submitButton.innerHTML = originalText;
                    }
                }, 15000);
            }
        });
    });
}

/* ===================================
   Auto-Hide Success/Info Alerts
   =================================== */
function initAutoHideAlerts() {
    const alerts = document.querySelectorAll('.alert-success, .alert-info');

    alerts.forEach(alert => {
        // Add fade-out animation after 5 seconds
        setTimeout(() => {
            alert.classList.add('fade');
            setTimeout(() => {
                const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
                bsAlert.close();
            }, 150);
        }, 5000);
    });
}

/* ===================================
   Release Date Toggle Based on Status
   =================================== */
function initReleaseDateToggle() {
    const statusSelect = document.getElementById('statusSelect');
    const releaseDateGroup = document.getElementById('releaseDateGroup');
    const releaseDateInput = document.getElementById('releaseDateInput');

    if (statusSelect && releaseDateGroup && releaseDateInput) {
        function toggleReleaseDate() {
            if (statusSelect.value === '3') { // Released
                releaseDateGroup.style.display = 'block';
                releaseDateInput.required = true;
                releaseDateGroup.classList.add('required-field');
            } else {
                releaseDateGroup.style.display = 'none';
                releaseDateInput.required = false;
                releaseDateInput.value = '';
                releaseDateGroup.classList.remove('required-field');
            }
        }

        // Listen for changes
        statusSelect.addEventListener('change', toggleReleaseDate);

        // Trigger on page load
        toggleReleaseDate();
    }
}

/* ===================================
   Client-Side Form Validation Enhancement
   =================================== */
function initFormValidation() {
    // Personal Number Validation (only digits, 11 characters)
    const personalNumberInputs = document.querySelectorAll('input[name="PersonalNumber"]');

    personalNumberInputs.forEach(input => {
        input.addEventListener('input', function (e) {
            // Remove non-digits
            this.value = this.value.replace(/\D/g, '');

            // Limit to 11 characters
            if (this.value.length > 11) {
                this.value = this.value.slice(0, 11);
            }
        });
    });

    // Email validation (real-time feedback)
    const emailInputs = document.querySelectorAll('input[type="email"]');

    emailInputs.forEach(input => {
        input.addEventListener('blur', function () {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            const isValid = emailRegex.test(this.value);

            if (this.value && !isValid) {
                this.classList.add('is-invalid');
            } else {
                this.classList.remove('is-invalid');
            }
        });
    });

    // Password confirmation validation
    const passwordInput = document.getElementById('Password');
    const confirmPasswordInput = document.getElementById('ConfirmPassword');

    if (passwordInput && confirmPasswordInput) {
        confirmPasswordInput.addEventListener('input', function () {
            if (this.value !== passwordInput.value) {
                this.setCustomValidity('პაროლები არ ემთხვევა');
                this.classList.add('is-invalid');
            } else {
                this.setCustomValidity('');
                this.classList.remove('is-invalid');
            }
        });

        passwordInput.addEventListener('input', function () {
            if (confirmPasswordInput.value && confirmPasswordInput.value !== this.value) {
                confirmPasswordInput.setCustomValidity('პაროლები არ ემთხვევა');
                confirmPasswordInput.classList.add('is-invalid');
            } else {
                confirmPasswordInput.setCustomValidity('');
                confirmPasswordInput.classList.remove('is-invalid');
            }
        });
    }

    // Date of Birth validation (18-100 years)
    const dateOfBirthInputs = document.querySelectorAll('input[name="DateOfBirth"]');

    dateOfBirthInputs.forEach(input => {
        input.addEventListener('change', function () {
            const birthDate = new Date(this.value);
            const today = new Date();
            let age = today.getFullYear() - birthDate.getFullYear();
            const monthDiff = today.getMonth() - birthDate.getMonth();

            if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
                age--;
            }

            if (age < 18 || age > 100) {
                this.setCustomValidity('ასაკი უნდა იყოს 18-დან 100 წლამდე');
                this.classList.add('is-invalid');
            } else {
                this.setCustomValidity('');
                this.classList.remove('is-invalid');
            }
        });
    });
}

/* ===================================
   Confirm Dialogs for Dangerous Actions
   =================================== */
function initConfirmDialogs() {
    // Already handled in views with inline onclick handlers
    // This is kept for potential future enhancements
}

/* ===================================
   Initialize Bootstrap Tooltips
   =================================== */
function initTooltips() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

/* ===================================
   Delete Modal Helper Function
   =================================== */
function showDeleteModal(employeeId, employeeName, personalNumber) {
    document.getElementById('employeeNameToDelete').textContent = employeeName;
    if (document.getElementById('employeePersonalNumberToDelete')) {
        document.getElementById('employeePersonalNumberToDelete').textContent = personalNumber;
    }
    document.getElementById('deleteForm').action = '/Employee/Delete/' + employeeId;

    const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
    modal.show();
}

/* ===================================
   Position Delete Confirmation
   =================================== */
function confirmDelete(positionName, employeeCount) {
    if (employeeCount > 0) {
        alert('თანამდებობის წაშლა შეუძლებელია! მას მიეკუთვნება ' + employeeCount + ' თანამშრომელი.');
        return false;
    }
    return confirm('ნამდვილად გსურთ თანამდებობის "' + positionName + '" წაშლა?');
}

/* ===================================
   Utility Functions
   =================================== */

// Show loading overlay
function showLoadingOverlay() {
    let overlay = document.querySelector('.loading-overlay');
    if (!overlay) {
        overlay = document.createElement('div');
        overlay.className = 'loading-overlay';
        overlay.innerHTML = '<div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div>';
        document.body.appendChild(overlay);
    }
    overlay.classList.add('show');
}

// Hide loading overlay
function hideLoadingOverlay() {
    const overlay = document.querySelector('.loading-overlay');
    if (overlay) {
        overlay.classList.remove('show');
    }
}

// Show toast notification
function showToast(message, type = 'success') {
    const toastContainer = document.querySelector('.toast-container') || createToastContainer();

    const toast = document.createElement('div');
    toast.className = `toast align-items-center text-white bg-${type} border-0`;
    toast.setAttribute('role', 'alert');
    toast.setAttribute('aria-live', 'assertive');
    toast.setAttribute('aria-atomic', 'true');

    toast.innerHTML = `
        <div class="d-flex">
            <div class="toast-body">
                ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
        </div>
    `;

    toastContainer.appendChild(toast);

    const bsToast = new bootstrap.Toast(toast);
    bsToast.show();

    // Remove toast element after it's hidden
    toast.addEventListener('hidden.bs.toast', function () {
        toast.remove();
    });
}

function createToastContainer() {
    const container = document.createElement('div');
    container.className = 'toast-container position-fixed top-0 end-0 p-3';
    container.style.zIndex = '9999';
    document.body.appendChild(container);
    return container;
}

/* ===================================
   Export functions for use in views
   =================================== */
window.showDeleteModal = showDeleteModal;
window.confirmDelete = confirmDelete;
window.showToast = showToast;
window.showLoadingOverlay = showLoadingOverlay;
window.hideLoadingOverlay = hideLoadingOverlay;