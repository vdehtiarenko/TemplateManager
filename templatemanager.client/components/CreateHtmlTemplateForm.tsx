import * as React from "react";
import { useState } from "react";
import "../src/styles/ModalForm.css";

interface Template {
    id: string;
    name: string;
    content: string;
}

interface CreateHtmlTemplateFormProps {
    onClose: () => void;
    onCreated?: (template: Template) => void;
}

const CreateHtmlTemplateForm: React.FC<CreateHtmlTemplateFormProps> = ({ onClose, onCreated }) => {
    const [name, setName] = useState("");
    const [content, setContent] = useState("");
    const [loading, setLoading] = useState(false);

    const handleSubmit: React.FormEventHandler<HTMLFormElement> = async (e) => {
        e.preventDefault();
        if (!name.trim() || !content.trim()) return;

        setLoading(true);

        try {
            const response = await fetch("/api/HtmlTemplates", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ name, content }),
            });

            if (!response.ok) {
                const errorText = await response.text();
                alert("Failed to create template: " + errorText);
                return;
            }

            const createdTemplate: Template = await response.json();
            if (onCreated) onCreated(createdTemplate);

            setName("");
            setContent("");
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
                <h2>Create New HTML Template</h2>
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

export default CreateHtmlTemplateForm;
