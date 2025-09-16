import * as React from "react";
import { useState } from "react";
import "../src/styles/ModalForm.css";

interface GeneratePdfFormProps {
    templateId: string;
    templateName: string;
    onClose: () => void;
}

const GeneratePdfForm: React.FC<GeneratePdfFormProps> = ({ templateId, templateName, onClose }) => {
    const [jsonInput, setJsonInput] = useState<string>("{}");
    const [loading, setLoading] = useState(false);

    const handleDownload = async () => {
        let placeholders: Record<string, string>;
        try {
            placeholders = JSON.parse(jsonInput);
        } catch {
            alert("Invalid JSON format.");
            return;
        }

        setLoading(true);
        try {
            const response = await fetch(`/api/HtmlTemplates/${templateId}/generate-pdf`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(placeholders),
            });

            if (!response.ok) {
                const errorText = await response.text();
                alert("Failed to generate PDF: " + errorText);
                return;
            }

            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement("a");
            a.href = url;
            a.download = `${templateName}.pdf`;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
            onClose();
        } catch {
            alert("An unexpected error occurred.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="generate-pdf-overlay" onClick={onClose}>
            <div className="generate-pdf-content" onClick={(e) => e.stopPropagation()}>
                <button className="close-btn" onClick={onClose}>✖</button>
                <div className="generate-pdf-title">Generate PDF for "{templateName}"</div>

                <div className="generate-pdf-json-section">
                    <label className="generate-pdf-json-label">Enter JSON placeholders:</label>
                    <textarea
                        className="generate-pdf-json-textarea"
                        value={jsonInput}
                        onChange={(e) => setJsonInput(e.currentTarget.value)}
                        placeholder='e.g. { "Name": "Іван", "Date": "2025-09-16" }'
                        disabled={loading}
                    />
                </div>

                <div className="form-buttons">
                    <button type="button" onClick={handleDownload} disabled={loading}>
                        {loading ? "Generating..." : "Generate PDF"}
                    </button>
                    <button type="button" onClick={onClose} disabled={loading}>
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    );
};

export default GeneratePdfForm;
