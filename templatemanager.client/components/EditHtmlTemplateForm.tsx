import * as React from "react";
import { useState, useEffect } from "react";
import "../src/styles/ModalForm.css";

interface Template {
    id: string;
    name: string;
    content: string;
}

interface EditHtmlTemplateFormProps {
    templateId: string;
    initialName: string;
    initialContent: string;
    onClose: () => void;
    onUpdated?: (template: Template) => void;
}

const EditHtmlTemplateForm: React.FC<EditHtmlTemplateFormProps> = ({
    templateId,
    initialName,
    initialContent,
    onClose,
    onUpdated
}) => {
    const [name, setName] = useState(initialName);
    const [content, setContent] = useState(initialContent);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        setName(initialName);
        setContent(initialContent);
    }, [initialName, initialContent]);

    const handleSubmit: React.FormEventHandler<HTMLFormElement> = async (e) => {
        e.preventDefault();
        if (!name.trim() || !content.trim()) return;

        setLoading(true);

        try {
            const response = await fetch(`/api/HtmlTemplates/${templateId}`, {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ id: templateId, name, content }),
            });

            if (!response.ok) {
                const errorText = await response.text();
                alert("Failed to update template: " + errorText);
                return;
            }

            const updatedTemplate: Template = await response.json();
            if (onUpdated) onUpdated(updatedTemplate);

            onClose();
        } catch (error) {
            console.error(error);
            alert("An unexpected error occurred.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="modal-overlay" onClick={onClose}>
            <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                <button className="close-btn" onClick={onClose}>✖</button>
                <h2>Edit HTML Template</h2>
                <form onSubmit={handleSubmit}>
                    <label>Template Name:</label>
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.currentTarget.value)}
                        placeholder="Enter template name"
                        required
                        disabled={loading}
                    />
                    <label>HTML Content:</label>
                    <textarea
                        value={content}
                        onChange={(e) => setContent(e.currentTarget.value)}
                        placeholder="Enter HTML content here"
                        required
                        disabled={loading}
                    />
                    <div className="form-buttons">
                        <button type="submit" disabled={loading}>
                            {loading ? "Saving..." : "Save"}
                        </button>
                        <button type="button" onClick={onClose} disabled={loading}>
                            Cancel
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
};

export default EditHtmlTemplateForm;


