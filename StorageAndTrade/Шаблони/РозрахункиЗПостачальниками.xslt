﻿<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" doctype-system="html" />

	<xsl:template name="Head">
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="Style/bootstrap.min.css" />
		<script src="Style/jquery.slim.min.js"></script>
		<script src="Style/popper.min.js"></script>
		<script src="Style/bootstrap.bundle.min.js"></script>
	</xsl:template>

    <xsl:template match="/root">

		<html>
			<head>
				<title>Table</title>
				<xsl:call-template name="Head" />
			</head>
			<body>

				<div class="container">

					<br/>
					<h2>Розрахунки з постачальниками</h2>
					<br/>

					<table class="table table-bordered table-sm">
						<tr class="table-success">
							<th>Контрагент</th>
							<th>Валюта</th>
							<th style="text-align:center">Сума</th>
						</tr>

						<xsl:for-each select="РозрахункиЗПостачальниками/row">
							<tr>
								<td>
									<xsl:value-of select="Контрагент_Назва"/>
								</td>
								<td>
									<xsl:value-of select="Валюта_Назва"/>
								</td>
								<td align="right">
									<xsl:value-of select="Сума"/>
								</td>
							</tr>
						</xsl:for-each>

					</table>

					<br/>
					<br/>
					<br/>
				    <br/>
				
				</div>

			</body>
		</html>
				
    </xsl:template>
	
</xsl:stylesheet>
