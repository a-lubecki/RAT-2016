/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

package com.lubecki.rat.synchronizer;

import java.util.Objects;

/**
 * Copyright Aurélien Lubecki 2013
 * @author Aurélien Lubecki
 */
public class ExtensionTransform {

    public final String inExtension;
    public final String outExtension;

    public ExtensionTransform(String extension) {
        this(extension, extension);
    }

    public ExtensionTransform(String inExtension, String outExtension) {

        this.inExtension = formatExtension(inExtension);
        this.outExtension = formatExtension(outExtension);

    }

    public static String formatExtension(String extension) {

        if(extension == null) {
            throw new IllegalArgumentException("Extension null");
        }

        if(!extension.startsWith(".")) {
            extension = "." + extension;
        }

        if(extension.length() < 2) {
            throw new IllegalArgumentException("Extension is too short : " + extension);
        }
        if(extension.lastIndexOf(".") != 0) {
            throw new IllegalArgumentException("Extension contains a dot not in the first position : " + extension);
        }

        return extension;
    }

    @Override
    public int hashCode() {
        int hash = 7;
        hash = 53 * hash + Objects.hashCode(this.inExtension);
        hash = 53 * hash + Objects.hashCode(this.outExtension);
        return hash;
    }

    @Override
    public boolean equals(Object obj) {
        if (obj == null) {
            return false;
        }
        if (getClass() != obj.getClass()) {
            return false;
        }
        final ExtensionTransform other = (ExtensionTransform) obj;
        if (!Objects.equals(this.inExtension, other.inExtension)) {
            return false;
        }
        if (!Objects.equals(this.outExtension, other.outExtension)) {
            return false;
        }
        return true;
    }


}
