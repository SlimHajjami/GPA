/*
* Kendo UI v2011.3.1129 (http://kendoui.com)
* Copyright 2011 Telerik AD. All rights reserved.
*
* Kendo UI commercial licenses may be obtained at http://kendoui.com/license.
* If you do not own a commercial license, this file shall be governed by the
* GNU General Public License (GPL) version 3. For GPL requirements, please
* review: http://www.gnu.org/copyleft/gpl.html
*/

﻿(function( window, undefined ) {
    kendo.cultures["fr-FR"] = {
        name: "fr-FR",
        numberFormat: {
            pattern: ["-n3"],
            decimals: 3,
            ",": " ",
            ".": ",",
            groupSize: [3],
            percent: {
                pattern: ["-n %","n %"],
                decimals: 3,
                ",": " ",
                ".": ",",
                groupSize: [3],
                symbol: "%"
            },
            currency: {
                pattern: ["-n $","n $"],
                decimals: 3,
                ",": " ",
                ".": ",",
                groupSize: [3],
                symbol: "€"
            }
        },
        calendars: {
            standard: {
                days: {
                    names: ["dimanche","lundi","mardi","mercredi","jeudi","vendredi","samedi"],
                    namesAbbr: ["dim.","lun.","mar.","mer.","jeu.","ven.","sam."],
                    namesShort: ["di","lu","ma","me","je","ve","sa"]
                },
                months: {
                    names: ["janvier","février","mars","avril","mai","juin","juillet","août","septembre","octobre","novembre","décembre",""],
                    namesAbbr: ["janv.","févr.","mars","avr.","mai","juin","juil.","août","sept.","oct.","nov.","déc.",""]
                },
                AM: [""],
                PM: [""],
                patterns: {
                    d: "dd/MM/yyyy",
                    D: "dddd d MMMM yyyy",
                    F: "dddd d MMMM yyyy HH:mm:ss",
                    g: "dd/MM/yyyy HH:mm",
                    G: "dd/MM/yyyy HH:mm:ss",
                    m: "d MMMM",
                    M: "d MMMM",
                    s: "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
                    t: "HH:mm",
                    T: "HH:mm:ss",
                    u: "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
                    y: "MMMM yyyy",
                    Y: "MMMM yyyy"
                },
                "/": "/",
                ":": ":",
                firstDay: 1
            }
        }
    }
})(this);

kendo.ui.Grid.prototype.options.messages =
$.extend(true, kendo.ui.Grid.prototype.options.messages, {
    "commands": {
        "create": "Insérer",
        "destroy": "Effacer",
        "canceledit": "Annuler",
        "update": "Mettre à jour",
        "edit": "Éditer",
        "excel": "Export to Excel",
        "pdf": "Export to PDF",
        "select": "Sélectionner",
        "cancel": "Annuler les modifications",
        "save": "Enregistrer les modifications"
    },
    "editable": {
        "confirmation": "Êtes-vous sûr de vouloir supprimer cet enregistrement?",
        "cancelDelete": "Annuler",
        "confirmDelete": "Effacer"
    },
    "noRecords": "Aucun enregistrement disponible."
});



kendo.ui.ColumnMenu.prototype.options.messages =
  $.extend(kendo.ui.ColumnMenu.prototype.options.messages, {

      /* COLUMN MENU MESSAGES 
       ****************************************************************************/
      sortAscending: "Trier a-z",
      sortDescending: "Trier z-a",
      filter: "Filtre",
      columns: "Colonnes"
      /***************************************************************************/
  });

kendo.ui.Groupable.prototype.options.messages =
  $.extend(kendo.ui.Groupable.prototype.options.messages, {

      /* GRID GROUP PANEL MESSAGES 
       ****************************************************************************/
      empty: "Glissez ici l'entête d'une colonne pour grouper les données",
          commands: {
              create: "Insérer",
              destroy: "Effacer",
              canceledit: "Annuler",
              update: "Mettre à jour",
              edit: "Éditer",
              excel: "Export to Excel",
              pdf: "Export to PDF",
              select: "Sélectionner",
              cancel: "Annuler les modifications",
              saveChanges: "Enregistrer les modifications"
          }
      /***************************************************************************/
  });

kendo.ui.FilterMenu.prototype.options.messages =
  $.extend(kendo.ui.FilterMenu.prototype.options.messages, {

      /* FILTER MENU MESSAGES 
       ***************************************************************************/
      info: "Montrer les éléments dont la valeur:", // sets the text on top of the filter menu
      isTrue: "est vrai",                   // sets the text for "isTrue" radio button
      isFalse: "n'est pas vrai",                 // sets the text for "isFalse" radio button
      filter: "Filtre",                    // sets the text for the "Filter" button
      clear: "Effacer",                      // sets the text for the "Clear" button
      and: "Et",
      or: "Ou",
      selectValue: "-Choisissez une valeur-"
      /***************************************************************************/
  });

kendo.ui.FilterMenu.prototype.options.operators =
  $.extend(kendo.ui.FilterMenu.prototype.options.operators, {

      /* FILTER MENU OPERATORS (for each supported data type) 
       ****************************************************************************/
      string: {
          eq: "Est égal à",
          neq: "N'est pas égal à",
          startswith: "Commence par",
          contains: "Contient",
          doesnotcontain: "Ne contient pas",
          endswith: "Fini par"
      },
      number: {
          eq: "Est égal à",
          neq: "N'est pas égal à",
          gte: "Est plus grand ou égal à",
          gt: "Est plus grand que",
          lte: "Est inférieur ou égal à",
          lt: "Est inférieur que"
      },
      date: {
          eq: "Est le ",
          neq: "N'est pas le",
          gte: "Est après ou le",
          gt: "Est après le",
          lte: "Est avant ou le",
          lt: "Est avant le"
      },
      enums: {
          eq: "Est égal à",
          neq: "N'est pas égal à",
      }
      /***************************************************************************/
  });

kendo.ui.Pager.prototype.options.messages =
  $.extend(kendo.ui.Pager.prototype.options.messages, {

      /* PAGER MESSAGES 
       ****************************************************************************/
      display: "{0} - {1} de {2} éléments",
      empty: "Aucun élément à afficher",
      page: "Page",
      of: "de {0}",
      itemsPerPage: "éléments par page",
      first: "Vers la première page",
      previous: "Vers la page précédente",
      next: "Vers la page suivante",
      last: "Vers la dernière page",
      refresh: "Rafraichir"
      /***************************************************************************/
  });

kendo.ui.Validator.prototype.options.messages =
  $.extend(kendo.ui.Validator.prototype.options.messages, {

      /* VALIDATOR MESSAGES 
       ****************************************************************************/
      required: "{0} est obligatoire",
      pattern: "{0} n'est pas valide",
      min: "{0} doit être plus grand ou égal à {1}",
      max: "{0} doit être plus petit ou égal à {1}",
      step: "{0} n'est pas valide",
      email: "{0} n'est pas une adresse mail valide",
      url: "{0} n'est pas un lien (URL) valide",
      date: "{0} n'est pas une date valide"
      /***************************************************************************/
  });

kendo.ui.ImageBrowser.prototype.options.messages =
  $.extend(kendo.ui.ImageBrowser.prototype.options.messages, {

      /* IMAGE BROWSER MESSAGES 
       ****************************************************************************/
      uploadFile: "Charger",
      orderBy: "Trier par",
      orderByName: "Nom",
      orderBySize: "Taille",
      directoryNotFound: "Aucun répértoire de ce nom.",
      emptyFolder: "Répertoire vide",
      deleteFile: 'Etes-vous sûr de vouloir supprimer "{0}"?',
      invalidFileType: "Le fichier sélectionné \"{0}\" n'est pas valide. Les type fichiers supportés sont {1}.",
      overwriteFile: "Un fichier du nom \"{0}\" existe déjà dans ce répertoire. Voulez-vous le remplacer?",
      dropFilesHere: "glissez les fichiers ici pour les charger"
      /***************************************************************************/
  });

kendo.ui.Editor.prototype.options.messages =
  $.extend(kendo.ui.Editor.prototype.options.messages, {

      /* EDITOR MESSAGES 
       ****************************************************************************/
      bold: "Gras",
      italic: "Italique",
      underline: "Sousligner",
      strikethrough: "Barrer",
      superscript: "Superscript",
      subscript: "Subscript",
      justifyCenter: "Centrer text",
      justifyLeft: "Aligner text gauche",
      justifyRight: "Aligner text droite",
      justifyFull: "Alligner gauche et droite",
      insertUnorderedList: "Inserer la liste dans le désordre",
      insertOrderedList: "Inserer la liste dans l'ordre",
      indent: "Indenter",
      outdent: "Désindenter",
      createLink: "Inserer hyperlien",
      unlink: "Supprimer hyperlien",
      insertImage: "Inserer image",
      insertHtml: "Inserer HTML",
      fontName: "Sélectionner police",
      fontNameInherit: "(hériter police)",
      fontSize: "Selectionner taille police",
      fontSizeInherit: "(hériter taille)",
      formatBlock: "Format",
      foreColor: "Couleur",
      backColor: "Couleur de fond",
      style: "Styles",
      emptyFolder: "Répertoire vide",
      uploadFile: "Charger",
      orderBy: "Trier par:",
      orderBySize: "Taille",
      orderByName: "Nom",
      invalidFileType: "Le fichier sélectionné \"{0}\" n'est pas valide. Les types de fichiers supportés sont {1}.",
      deleteFile: 'Etes-vous sûr de vouloir supprimer le fichier "{0}"?',
      overwriteFile: 'Un fichier du nom "{0}" existe déjà dans ce répertoire. Voulez-vous le remplacer?',
      directoryNotFound: "Aucun répertoire de ce nom.",
      imageWebAddress: "Adress web",
      imageAltText: "Texte alternatif",
      dialogInsert: "Insérer",
      dialogButtonSeparator: "ou",
      dialogCancel: "Abandonner"
      /***************************************************************************/
  });
